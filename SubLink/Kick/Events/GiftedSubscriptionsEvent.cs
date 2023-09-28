using System;
using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class GiftedSubscriptionsEvent {
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("chatroom_id")]
    public uint ChatroomId { get; set; } = 0;

    [JsonProperty("gifted_usernames")]
    public string[] Users { get; set; } = Array.Empty<string>();

    [JsonProperty("gifter_username")]
    public string Gifter { get; set; } = string.Empty;

    public int GetGiftCount() =>
        Users.Length;
}
