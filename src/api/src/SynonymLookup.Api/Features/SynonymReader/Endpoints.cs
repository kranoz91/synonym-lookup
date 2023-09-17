using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResultNet;
using SynonymLookup.Api.Features.SynonymReader.Commands;
using SynonymLookup.Api.Features.SynonymReader.Models;

namespace SynonymLookup.Api.Features.SynonymReader;

public static class Endpoints
{
    public static WebApplication MapSynonymReader(this WebApplication app)
    {
        MapGetSynonyms(app);
        return app;
    }

    private static void MapGetSynonyms(WebApplication app)
    {
        var api = app.MapGet(
            "/v1/words/{word}/synonyms",
            async ([FromServices] IMediator mediator, [FromRoute] string word) =>
            {
                var result = await mediator.Send(new GetSynonymsCommand(new Word(word)));

                if (result.IsFailure())
                {
                    return Errors.ResolveError(result);
                }

                return Results.Ok(result.Data);
            })
            .WithName("GetSynonyms")
            .WithOpenApi();
    }
}