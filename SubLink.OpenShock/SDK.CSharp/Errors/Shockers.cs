using System;

namespace OpenShock.SDK.CSharp.Errors;

public struct ShockerNotFoundOrNoAccess(Guid value)
{
    public Guid Value { get; } = value;
}

public struct ShockerPaused(Guid value)
{
    public Guid Value { get; } = value;
}

public struct ShockerNoPermission(Guid value)
{
    public Guid Value { get; } = value;
}