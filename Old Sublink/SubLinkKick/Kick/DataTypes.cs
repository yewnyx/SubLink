using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick;

public sealed class KickBadge {
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("count"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Count { get; set; } = 0;
}

public sealed class KickIdentity {
    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("badges")]
    public KickBadge[] Badges { get; set; } = Array.Empty<KickBadge>();
}

public sealed class KickUser {
    [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Id { get; set; } = 0;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("identity")]
    public KickIdentity Identity { get; set; } = new();
}

public sealed class KickUserShort {
    [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Id { get; set; } = 0;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;
}
