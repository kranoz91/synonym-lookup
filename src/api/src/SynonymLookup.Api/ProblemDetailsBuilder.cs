using Microsoft.AspNetCore.Mvc;
using SynonymLookup.Api.Extensions;
using System.Net;

namespace SynonymLookup.Api;

internal class ProblemDetailsBuilder
{
    private ProblemDetails _problemDetails;

    internal ProblemDetailsBuilder(string errorCode)
    {
        _problemDetails = new ProblemDetails();
        _problemDetails.Extensions.Add(new(PredefinedErrorRegister.ErrorCodePropertyName, errorCode));
    }

    internal ProblemDetailsBuilder(ProblemDetails problemDetails)
    {
        _problemDetails = new ProblemDetails();
        _problemDetails.Extensions.AddRange(problemDetails.Extensions);
        WithTitle(problemDetails.Title ?? string.Empty);
        WithType(problemDetails.Type ?? string.Empty);
        WithStatusCode(problemDetails.Status ?? (int)HttpStatusCode.InternalServerError);
    }

    internal ProblemDetailsBuilder WithTitle(string title)
    {
        _problemDetails.Title = title;
        return this;
    }

    internal ProblemDetailsBuilder WithStatusCode(int statusCode)
    {
        _problemDetails.Status = statusCode;
        return this;
    }

    internal ProblemDetailsBuilder WithType(string typeLink)
    {
        _problemDetails.Type = typeLink;
        return this;
    }

    internal ProblemDetails Build() => _problemDetails;
}
