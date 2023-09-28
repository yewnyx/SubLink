using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class StreamHostEvent {
    [JsonProperty("chatroom_id")]
    public uint ChatroomId { get; set; } = 0;

    [JsonProperty("optional_message")]
    public string OptionalMessage { get; set; } = string.Empty;

    [JsonProperty("number_viewers")]
    public uint NumberViewers { get; set; } = 0;

    [JsonProperty("host_username")]
    public string HostUsername { get; set; } = string.Empty;
}
