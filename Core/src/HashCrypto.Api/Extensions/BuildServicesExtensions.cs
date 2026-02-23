using HashCrypto.Api.Services;

namespace HashCrypto.Api.Extensions;

public static class BuildServicesExtensions
{
    public static void InsertDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<IHashService, HashService>();
        services.AddTransient<ICryptoService, CryptoService>();
    }
}