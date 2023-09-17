using Microsoft.Extensions.DependencyInjection;

namespace SynonymLookup.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services)
    {
        var database = new WordDatabase();

        services.AddSingleton<IWordReader>(database);
        services.AddSingleton<IWordWriter>(database);

        return services;
    }
}
