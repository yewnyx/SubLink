using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.FanslyClient.Events;

public sealed class TipEvent {
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

    [JsonPropertyName("amount")]
    public float Amount { get; set; } = 0f;

    [JsonPropertyName("centAmount")]
    public int CentAmount { get; set; } = 0;

    [JsonPropertyName("rawAmount")]
    public uint RawAmount { get; set; } = 0;

    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; } = 0;

    public TipEvent() { }

    public TipEvent(string id, string chatRoomId, string senderId, string username, string displayname, string content, uint rawAmount, long createdAt) {
        (Id, ChatRoomId, SenderId, Username, Displayname, Content, RawAmount, CreatedAt) = (id, chatRoomId, senderId, username, displayname, content, rawAmount, createdAt);
        Amount = (float)Math.Round(rawAmount / 1000d, 2, MidpointRounding.AwayFromZero);
        CentAmount = (int)Math.Round(rawAmount / 10d, 0, MidpointRounding.AwayFromZero);
    }

    public TipEvent(string id, string chatRoomId, string senderId, string username, string displayname, string content, float amount, int centAmount, uint rawAmount, long createdAt) =>
        (Id, ChatRoomId, SenderId, Username, Displayname, Content, Amount, CentAmount, RawAmount, CreatedAt) = (id, chatRoomId, senderId, username, displayname, content, amount, centAmount, rawAmount, createdAt);
}
