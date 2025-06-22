using System;

namespace OpenShock.SDK.CSharp.Models;

public sealed class SelfResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required Uri Image { get; set; }
    public required RankType Rank { get; set; }
}