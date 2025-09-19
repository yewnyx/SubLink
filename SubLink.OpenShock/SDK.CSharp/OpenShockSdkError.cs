using System;

namespace OpenShock.SDK.CSharp;

public sealed class OpenShockSdkError(string message) : Exception(message) { }