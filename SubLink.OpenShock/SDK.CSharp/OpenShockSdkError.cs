using System;

namespace OpenShock.SDK.CSharp;

public sealed class OpenShockSdkError : Exception
{
    public OpenShockSdkError(string message) : base(message)
    {
    }
}