using System.Text;
using HashCrypto.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HashCrypto.Api.Routes;

public static class CryptoRoutes
{
    public static WebApplication WebCryptoRoutes(this WebApplication app)
    {
        app.MapPost("/EncryptedText/{secret}/{inputText}", (ICryptoService cryptoService, string secret, string inputText) =>
            {
                try
                {
                    string encryptedText = cryptoService.GetEncryptedText(inputText, secret);
                    return Results.Ok(encryptedText);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/DecryptedText", (ICryptoService cryptoService, [FromBody] DecryptedInput input) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(input.InputText))
                        return Results.BadRequest("Input is null");

                    string decryptedText = cryptoService.GetDecryptedText(Encoding.Default.GetBytes(input.InputText), input.SharedSecret);
                    return Results.Ok(decryptedText);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}