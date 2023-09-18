using Microsoft.AspNetCore.Mvc;
using ResultNet;

namespace SynonymLookup.Api.Features.SynonymReader;

internal static class Errors
{
    internal const string SL101 = "When trying to get synonyms, group could not be found.";
    
    // Because of the way that adding new words and synonyms is designed, SL102 error should never occur.
    // If groupId for input word cannot be found, then SL101 will be returned.
    // If groupId is found, then at least the input word should exist within the group.
    // This error would only occur if there is a bug in how to create words and synonyms.
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
        var error = PredefinedErrors.GetValueOrDefault(result.Code);
        if (error == null)
        {
            return Results.Problem(PredefinedErrorRegister.Fallback);
        }

        return Results.Problem(error);
    }
}
