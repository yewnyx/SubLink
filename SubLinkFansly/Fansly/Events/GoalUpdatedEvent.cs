using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.Events;

public sealed class GoalUpdatedEvent {
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
    public int CurrentAmount { get; set; } = 0;

    [JsonPropertyName("goalAmount")]
    public int GoalAmount { get; set; } = 0;

    [JsonPropertyName("version")]
    public uint Version { get; set; } = 0;

    public GoalUpdatedEvent() { }

    public GoalUpdatedEvent(string id, string chatRoomId, string accountId, uint type, string label, string description, uint status, int currentAmount, int goalAmount, uint version) {
        Id = id;
        ChatRoomId = chatRoomId;
        AccountId = accountId;
        Type = type;
        Label = label;
        Description = description;
        Status = status;
        CurrentAmount = currentAmount;
        GoalAmount = goalAmount;
        Version = version;
    }
}
