using System;
using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick;

public sealed class KickBadge {
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;

    [JsonProperty("count")]
    public int Count { get; set; } = 0;
}

public sealed class KickIdentity {
    [JsonProperty("color")]
    public string Color { get; set; } = string.Empty;

    [JsonProperty("badges")]
    public KickBadge[] Badges { get; set; } = Array.Empty<KickBadge>();
}

public sealed class KickUser {
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonProperty("identity")]
    public KickIdentity Identity { get; set; } = new();
}

public sealed class KickUserShort {
    [JsonProperty("id")]
    public uint Id { get; set; } = 0;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;
}
