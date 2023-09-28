using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class ChatroomClearEvent {
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
}
