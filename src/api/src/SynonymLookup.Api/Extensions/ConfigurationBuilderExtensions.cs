using Azure.Identity;

namespace SynonymLookup.Api.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAzureAppConfiguration(this IConfigurationBuilder builder, Uri appConfigUri, string applicationName, string environmentName) =>
        builder.AddAzureAppConfiguration(options =>
        {
            var credentials = new DefaultAzureCredential();

            options
                .Connect(appConfigUri, credentials)
                // Load configuration values with no label
                .Select(keyFilter: $"{applicationName}*")
                // Override with any configuration values specific to current hosting env
                .Select(keyFilter: $"{applicationName}*", labelFilter: environmentName)
                // Trim application prefixes.
                .TrimKeyPrefix($"{applicationName}:")
                // Support feature management
                .UseFeatureFlags(options => options.Select($"{applicationName}*"))
                // Configures the provider to use the provided KeyVault configuration to resolve key vault references if any.
                .ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(credentials);
                });
        });
}