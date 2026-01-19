using System.Security.Cryptography;
using System.Text;

namespace HashCrypto.Api.Services;

public class CryptoService : ICryptoService
{
    private static readonly byte[] Salt = RandomNumberGenerator.GetBytes(16);

    public string GetEncryptedText(string text, string sharedSecret) => EncryptStringAes(text, sharedSecret);
    public string GetDecryptedText(byte[] inBuffer, string sharedSecret) => DecryptStringAes(inBuffer, sharedSecret);

    private static string EncryptStringAes(string plainText, string sharedSecret)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText));
        if (string.IsNullOrEmpty(sharedSecret))
            throw new ArgumentNullException(nameof(sharedSecret));

        string? outStr = null;// Encrypted string to return
        Aes? aesAlg = null;// RijndaelManaged object used to encrypt the data.

        try
        {
            const int iterations = 100_000;// Número robusto de iterações
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                sharedSecret,
                Salt,
                iterations,
                HashAlgorithmName.SHA256,// Força o uso de um hash moderno
                32// Tamanho da chave de saída (ex: 256 bits)
            );

            aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.Padding = PaddingMode.Zeros;
            // Create a decryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            MemoryStream? msEncrypt = new();

            msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

            CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);

            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                //Write all data to the stream.
                swEncrypt.Write(plainText);
            }

            outStr = Convert.ToBase64String(msEncrypt.ToArray());
        }
        finally
        {
            // Clear the RijndaelManaged object.
            aesAlg?.Clear();
        }

        // Return the encrypted bytes from the memory stream.
        return outStr;
    }

    private static string DecryptStringAes(byte[] inBuffer, string sharedSecret)
    {
        if (inBuffer == null || inBuffer.Length == 0)
            throw new ArgumentNullException(nameof(inBuffer));
        if (string.IsNullOrEmpty(sharedSecret))
            throw new ArgumentNullException(nameof(sharedSecret));

        // used to decrypt the data.
        Aes? aesAlg = null;
        // Declare the string used to hold
        // the decrypted text.
        byte[]? outBuffer = null;

        try
        {
            // generate the key from the shared secret and the salt
            //Rfc2898DeriveBytes key = new (sharedSecret, Salt);

            const int iterations = 100_000;// Número robusto de iterações
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                sharedSecret,
                Salt,
                iterations,
                HashAlgorithmName.SHA256,// Força o uso de um hash moderno
                32// Tamanho da chave de saída (ex: 256 bits)
            );


            byte[] preBuffer = Convert.FromBase64String(Encoding.Default.GetString(inBuffer));
            // Create the streams used for decryption.

            MemoryStream msDecrypt = new(preBuffer);

            // with the specified key and IV.
            aesAlg = Aes.Create();
            aesAlg.Key = key;

            // Get the initialization vector from the encrypted stream
            aesAlg.IV = ReadByteArray(msDecrypt);
            aesAlg.Padding = PaddingMode.Zeros;
            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            outBuffer = Encoding.Default.GetBytes(srDecrypt.ReadToEnd());

        }
        finally
        {
            aesAlg?.Clear();
        }

        return Encoding.Default.GetString(outBuffer).TrimEnd('\0');
    }

    private static byte[] ReadByteArray(Stream s)
    {
        byte[] rawLength = new byte[sizeof(int)];
        if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
        {
            throw new IOException("Stream did not contain properly formatted byte array");
        }

        byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
        return s.Read(buffer, 0, buffer.Length) != buffer.Length ? throw new IOException("Did not read byte array properly") : buffer;
    }
}