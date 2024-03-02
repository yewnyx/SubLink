using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.KickClient.Events;

public sealed class UserBannedEvent {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public KickUserShort User { get; set; } = new();

    [JsonPropertyName("banned_by")]
    public KickUserShort BannedBy { get; set; } = new();

    [JsonPropertyName("expires_at")]
    public string ExpiresAt { get; set; } = string.Empty;
}
