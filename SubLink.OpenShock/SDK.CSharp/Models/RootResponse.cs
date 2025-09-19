using System;

namespace OpenShock.SDK.CSharp.Models;

public sealed class RootResponse {
    public required string Version { get; set; }
    public required string Commit { get; set; }
    public required DateTimeOffset CurrentTime { get; set; }
    public required Uri FrontendUrl { get; set; }
    public required Uri ShortLinkUrl { get; set; }
}