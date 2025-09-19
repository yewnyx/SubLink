using System.Collections.Generic;

namespace OpenShock.SDK.CSharp.Models;

public sealed class ControlRequest {
    public required IEnumerable<Control> Shocks { get; set; }
    public required string? CustomName { get; set; }
}