using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class ChatMessageEvent {
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("chatroom_id")]
    public uint ChatroomId { get; set; } = 0;

    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonProperty("sender")]
    public KickUser Sender { get; set; } = new();
}
