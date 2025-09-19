using System;
using System.Net;
using OpenShock.SDK.CSharp.Problems;

namespace OpenShock.SDK.CSharp;

public sealed class OpenShockApiError : Exception
{
    public OpenShockApiError(string message, HttpStatusCode statusCode) :
        base($"{message} (HTTP {(int)statusCode})") { }

    public OpenShockApiError(string message, ProblemDetails problemDetails) :
        base($"{message} (HTTP {problemDetails.Status}; {problemDetails.Title}; {problemDetails.Detail})") =>
        ProblemDetails = problemDetails;
    
    public ProblemDetails? ProblemDetails { get; }
}