using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class ChatroomClearEvent {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
