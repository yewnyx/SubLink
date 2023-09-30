using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class StreamHostEvent {
    [JsonPropertyName("chatroom_id")]
    public uint ChatroomId { get; set; } = 0;

    [JsonPropertyName("optional_message")]
    public string OptionalMessage { get; set; } = string.Empty;

    [JsonPropertyName("number_viewers")]
    public uint NumberViewers { get; set; } = 0;

    [JsonPropertyName("host_username")]
    public string HostUsername { get; set; } = string.Empty;
}
