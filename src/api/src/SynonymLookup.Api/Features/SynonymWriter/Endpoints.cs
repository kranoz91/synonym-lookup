
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResultNet;
using SynonymLookup.Api.Extensions;
using SynonymLookup.Api.Features.SynonymWriter.Commands;
using SynonymLookup.Api.Features.SynonymWriter.Models;

namespace SynonymLookup.Api.Features.SynonymWriter;

public static class Endpoints
{
    public static WebApplication MapSynonymWriter(this WebApplication app)
    {
        MapPostWord(app);
        return app;
    }

    private static void MapPostWord(WebApplication app)
    {
        var api = app.MapPost(
            "/v1/words",
            async ([FromServices] IMediator mediator, [FromBody] CreateWordDTO input) =>
            {
                var word = new Word(input.Word);
                if (word.IsValid(out var validationResult) == false)
                {
                    return Errors.ResolveError(validationResult);
                }

                var synonyms = new List<Word>();
                foreach (var synonym in input.Synonyms)
                {
                    var wrappedSynonym = new Word(synonym);
                    if (wrappedSynonym.IsValid(out validationResult) == false)
                    {
                        return Errors.ResolveError(validationResult);
                    }

                    synonyms.Add(wrappedSynonym);
                }

                var result = await mediator.Send(new CreateWordCommand(word, synonyms.ToArray()));

                if (result.IsFailure())
                {
                    return Errors.ResolveError(result);
                }

                return Results.Created($"/v1/words/{input.Word}/synonyms", null);
            })
            .WithName("PostWord")
            .WithOpenApi();
    }
}
