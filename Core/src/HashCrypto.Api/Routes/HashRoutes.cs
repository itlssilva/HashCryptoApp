using HashCrypto.Api.Services;

namespace HashCrypto.Api.Routes;

public static class HashRoutes
{
    public static WebApplication WebHashRoutes(this WebApplication app)
    {
        app.MapPost("/GetHash/{text}", (IHashService hashService, string text) =>
        {
            try
            {
                string hashText = hashService.GetHash(text);
                return Results.Ok(hashText);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);


        return app;
    }
}