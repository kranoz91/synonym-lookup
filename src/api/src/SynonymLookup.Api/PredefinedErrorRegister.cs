using Microsoft.AspNetCore.Mvc;
using SynonymLookup.Api.Extensions;

namespace SynonymLookup.Api;

internal static class PredefinedErrorRegister
{
    internal const string ErrorCodePropertyName = "errorCode";
    internal const string BadRequestTypeDocumentationLink = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    internal const string NotFoundTypeDocumentationLink = "https://tools.ietf.org/html/rfc7231#section-6.5.4";

    internal const string SL999 = "Unhandled error.";
    internal static readonly ProblemDetails Fallback =
        new ProblemDetailsBuilder(nameof(SL999))
            .WithTitle(SL999)
            .WithStatusCode(500)
            .Build();

    internal static readonly IDictionary<string, ProblemDetails> PredefinedErrors =
        new Dictionary<string, ProblemDetails>()
            .AddRange(Features.SynonymReader.Errors.PredefinedErrors)
            .AddRange(Features.SynonymWriter.Errors.PredefinedErrors);
}
