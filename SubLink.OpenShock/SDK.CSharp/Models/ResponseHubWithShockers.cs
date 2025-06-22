using System.Collections.Generic;

namespace OpenShock.SDK.CSharp.Models;

public sealed class ResponseHubWithShockers : ResponseHub
{
    public required IEnumerable<ShockerResponse> Shockers { get; set; }
}