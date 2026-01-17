using System.Security.Cryptography;
using System.Text;

namespace HashCrypto.Api.Services;

public class HashService: IHashService
{
    public string GetHash(string input)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = SHA256.HashData(bytes);

        StringBuilder sb = new();
        foreach (byte t in hash)
            sb.Append(t.ToString("X2"));

        return sb.ToString();
    }
}