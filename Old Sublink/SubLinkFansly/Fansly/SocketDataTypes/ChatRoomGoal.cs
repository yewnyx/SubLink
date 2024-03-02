using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.SocketDataTypes;

internal sealed class ChatRoomGoal : BaseEventType {
    public sealed class InnerData {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("chatRoomId")]
        public string ChatRoomId { get; set; } = string.Empty;

        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public uint Type { get; set; } = 0;

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public uint Status { get; set; } = 0;

        [JsonPropertyName("currentAmount")]
        public ulong CurrentAmount { get; set; } = 0;

        [JsonPropertyName("goalAmount")]
        public ulong GoalAmount { get; set; } = 0;

        [JsonPropertyName("version")]
        public uint Version { get; set; } = 0;
    }

    [JsonPropertyName("chatRoomGoal")]
    public InnerData Data { get; set; } = new();

    public ChatRoomGoal() { }
}
