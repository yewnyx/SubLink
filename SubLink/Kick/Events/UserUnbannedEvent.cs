using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class UserUnbannedEvent {
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("user")]
    public KickUserShort User { get; set; } = new();

    [JsonProperty("unbanned_by")]
    public KickUserShort UnbannedBy { get; set; } = new();
}
