using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class ChatMessageEvent {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("chatroom_id")]
    public uint ChatroomId { get; set; } = 0;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("sender")]
    public KickUser Sender { get; set; } = new();
}
