namespace SynonymLookup.Api.Features.WeatherForecast;

public static class Configuration
{
    public static WebApplicationBuilder BindWeatherConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<WeatherConfiguration>(builder.Configuration.GetSection(nameof(WeatherConfiguration)));
        return builder;
    }
}