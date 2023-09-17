using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ResultNet;
using SynonymLookup.Api.Features.SynonymWriter.Validators;

namespace SynonymLookup.Api.Features.SynonymWriter;

internal static class Errors
{
    internal const string SL201 = "Input words can not be empty.";
    internal static readonly string SL202 = $"Words can not exceed the maximum of {WordValidator.MAX_LENGTH} characters.";
    internal const string SL203 = "Words can only consist of letters A to Z.";

    internal static readonly Dictionary<string, ProblemDetails> PredefinedErrors = new()
    {
        {
            nameof(SL201),
            new ProblemDetailsBuilder(nameof(SL201))
                .WithTitle(SL201)
                .WithStatusCode(400)
                .WithType(PredefinedErrorRegister.BadRequestTypeDocumentationLink)
                .Build()
        },
        {
            nameof(SL202),
            new ProblemDetailsBuilder(nameof(SL202))
                .WithTitle(SL202)
                .WithStatusCode(400)
                .WithType(PredefinedErrorRegister.BadRequestTypeDocumentationLink)
                .Build()
        },
        {
            nameof(SL203),
            new ProblemDetailsBuilder(nameof(SL203))
                .WithTitle(SL203)
                .WithStatusCode(400)
                .WithType(PredefinedErrorRegister.BadRequestTypeDocumentationLink)
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

    internal static IResult ResolveError(ValidationResult result)
    {
        var error = PredefinedErrors[result.Errors.FirstOrDefault()?.ErrorCode ?? string.Empty];
        if (error == null)
        {
            return Results.Problem(PredefinedErrorRegister.Fallback);
        }

        return Results.Problem(error);
    }
}
