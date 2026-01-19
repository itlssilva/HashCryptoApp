namespace HashCrypto.Api.Services;

public interface ICryptoService
{
    string GetEncryptedText(string text, string sharedSecret);
    string GetDecryptedText(byte[] inBuffer, string sharedSecret);
}