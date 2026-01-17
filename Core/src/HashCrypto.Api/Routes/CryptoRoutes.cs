using HashCrypto.Api.Services;

namespace HashCrypto.Api.Routes;

public static class CryptoRoutes
{
    public static WebApplication WebCryptoRoutes(this WebApplication app)
    {
        app.MapPost("/GetEncryptText/{secret}/{inputText}", (ICryptoService cryptoService, string secret, string inputText) =>
        {
            try
            {
                string encryptText = cryptoService.GetCryptoText(inputText,  secret);
                return Results.Ok(encryptText);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        return app;
    }
}