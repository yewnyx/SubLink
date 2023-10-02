using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class MessageDeletedEvent {
    public sealed class MessageInfo {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public MessageInfo Message { get; set; } = new();
}
