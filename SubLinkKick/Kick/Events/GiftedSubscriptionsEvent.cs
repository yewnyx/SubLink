using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class GiftedSubscriptionsEvent {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("chatroom_id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint ChatroomId { get; set; } = 0;

    [JsonPropertyName("gifted_usernames")]
    public string[] Users { get; set; } = Array.Empty<string>();

    [JsonPropertyName("gifter_username")]
    public string Gifter { get; set; } = string.Empty;

    public int GetGiftCount() =>
        Users.Length;
}
