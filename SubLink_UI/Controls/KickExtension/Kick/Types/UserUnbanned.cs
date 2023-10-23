using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class UserUnbanned {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public KickUserShort User { get; set; } = new();

    [JsonPropertyName("unbanned_by")]
    public KickUserShort UnbannedBy { get; set; } = new();
}
