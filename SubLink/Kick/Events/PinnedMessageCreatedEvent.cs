using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class PinnedMessageCreatedEvent {
    public sealed class MessageInfo {
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

        [JsonProperty("metadata")]
        public object Metadata { get; set; } = new();
    }

    [JsonProperty("message")]
    public MessageInfo Message { get; set; } = new();

    [JsonProperty("duration")]
    public string Duration { get; set; } = string.Empty;
}
