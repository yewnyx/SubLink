using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class MessageDeletedEvent {
    public sealed class MessageInfo {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
    }

    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("message")]
    public MessageInfo Message { get; set; } = new();
}
