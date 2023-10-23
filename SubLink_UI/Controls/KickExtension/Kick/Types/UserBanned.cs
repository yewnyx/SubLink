using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class UserBanned {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public KickUserShort User { get; set; } = new();

    [JsonPropertyName("banned_by")]
    public KickUserShort BannedBy { get; set; } = new();

    [JsonPropertyName("expires_at")]
    public string ExpiresAt { get; set; } = string.Empty;
}
