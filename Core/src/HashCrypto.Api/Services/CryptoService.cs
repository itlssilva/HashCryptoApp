using System.Security.Cryptography;

namespace HashCrypto.Api.Services;

public class CryptoService : ICryptoService
{
    private static readonly byte[] Salt = RandomNumberGenerator.GetBytes(16);

    public string GetCryptoText(string text, string sharedSecret) => EncryptStringAes(text, sharedSecret);

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
            const int iterations = 100_000; // Número robusto de iterações
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                sharedSecret,
                Salt,
                iterations,
                HashAlgorithmName.SHA256, // Força o uso de um hash moderno
                32 // Tamanho da chave de saída (ex: 256 bits)
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
}