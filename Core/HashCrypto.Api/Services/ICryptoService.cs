namespace HashCrypto.Api.Services;

public interface ICryptoService
{
    string GetCryptoText(string text, string sharedSecret);
}