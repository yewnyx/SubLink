using System;

namespace OpenShock.SDK.CSharp.Errors;

public readonly struct ShockerNotFoundOrNoAccess(Guid value) {
    public Guid Value { get; } = value;
}

public readonly struct ShockerPaused(Guid value) {
    public Guid Value { get; } = value;
}

public readonly struct ShockerNoPermission(Guid value) {
    public Guid Value { get; } = value;
}