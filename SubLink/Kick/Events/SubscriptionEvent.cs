using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class SubscriptionEvent {
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("chatroom_id")]
    public uint ChatroomId { get; set; } = 0;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("months")]
    public uint Months { get; set; } = 0;
}
