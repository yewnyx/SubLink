using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.FanslyClient.Events;

public sealed class ChatMessageEvent {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("chatRoomId")]
    public string ChatRoomId { get; set; } = string.Empty;

    [JsonPropertyName("senderId")]
    public string SenderId { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("displayname")]
    public string Displayname { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; } = 0;

    public ChatMessageEvent() { }

    public ChatMessageEvent(string id, string chatRoomId, string senderId, string username, string displayname, string content, long createdAt) {
        Id = id;
        ChatRoomId = chatRoomId;
        SenderId = senderId;
        Username = username;
        Displayname = displayname;
        Content = content;
        CreatedAt = createdAt;
    }
}
