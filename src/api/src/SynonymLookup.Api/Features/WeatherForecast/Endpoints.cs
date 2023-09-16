using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web.Resource;
using System.Text.Json;

namespace SynonymLookup.Api.Features.WeatherForecast;

public static class Endpoints
{
    public static WebApplication MapWeatherForecast(this WebApplication app, string requiredScope)
    {
        MapGetWeatherForecast(app, requiredScope);
        return app;
    }

    private static void MapGetWeatherForecast(WebApplication app, string requiredScope)
    {
        var api = app.MapGet(
            "/weatherforecast",
            async ([FromServices] IOptionsSnapshot<WeatherConfiguration> options, HttpContext httpContext
            ) =>
            {
                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };
                var config = options.Value;

                httpContext.VerifyUserHasAnyAcceptedScope(requiredScope);

                var forecasts = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(config.MinimumTemperature, config.MaximumTemperature),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();

                try
                {
                    return new WeatherForecastResponse(forecasts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create signature. Exception: {JsonSerializer.Serialize(ex)}");
                    throw;
                }

            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();
        api.RequireAuthorization();
    }
}