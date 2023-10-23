using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class ChatroomClear {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
