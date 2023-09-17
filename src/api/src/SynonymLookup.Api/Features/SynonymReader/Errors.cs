using Microsoft.AspNetCore.Mvc;
using ResultNet;

namespace SynonymLookup.Api.Features.SynonymReader;

internal static class Errors
{
    internal const string SL101 = "When trying to get synonyms, group could not be found.";
    internal const string SL102 = "When trying to get synonyms, group was empty.";

    internal static readonly Dictionary<string, ProblemDetails> PredefinedErrors = new()
    {
        {
            nameof(SL101),
            new ProblemDetailsBuilder(nameof(SL101))
                .WithTitle(SL101)
                .WithStatusCode(404)
                .WithType(PredefinedErrorRegister.NotFoundTypeDocumentationLink)
                .Build()
        },
        {
            nameof(SL102),
            new ProblemDetailsBuilder(nameof(SL102))
                .WithTitle(SL102)
                .WithStatusCode(404)
                .WithType(PredefinedErrorRegister.NotFoundTypeDocumentationLink)
                .Build()
        }
    };

    internal static IResult ResolveError(Result result)
    {
        var error = PredefinedErrors[result.Code];
        if (error == null)
        {
            return Results.Problem(PredefinedErrorRegister.Fallback);
        }

        return Results.Problem(error);
    }
}
