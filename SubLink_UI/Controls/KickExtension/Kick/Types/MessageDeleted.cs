using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class MessageDeleted {
    public sealed class MessageInfo {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public MessageInfo Message { get; set; } = new();
}
