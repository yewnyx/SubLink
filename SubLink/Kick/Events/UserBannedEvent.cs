using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class UserBannedEvent {
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("user")]
    public KickUserShort User { get; set; } = new();

    [JsonProperty("banned_by")]
    public KickUserShort BannedBy { get; set; } = new();

    [JsonProperty("expires_at")]
    public string ExpiresAt { get; set; } = string.Empty;
}
