using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.KickClient.Events;

public sealed class UserUnbannedEvent {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public KickUserShort User { get; set; } = new();

    [JsonPropertyName("unbanned_by")]
    public KickUserShort UnbannedBy { get; set; } = new();
}
