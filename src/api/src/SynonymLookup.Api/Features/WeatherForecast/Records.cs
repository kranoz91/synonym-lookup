namespace SynonymLookup.Api.Features.WeatherForecast;

internal record WeatherForecastResponse(ICollection<WeatherForecast> Forecasts);

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record WeatherConfiguration
{
    public int MinimumTemperature { get; init; }
    public int MaximumTemperature { get; init; }
};