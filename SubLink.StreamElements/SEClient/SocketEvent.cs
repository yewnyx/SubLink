using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.StreamElements.SEClient;

internal sealed class SocketEvent {
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("provider")]
    public string Provider { get; set; } = string.Empty;

    [JsonPropertyName("channel")]
    public string ChannelId { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("data"), JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
    public Dictionary<string, object?> Data { get; set; } = new();

    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("updatedAt")]
    public string UpdatedAt { get; set; } = string.Empty;

    [JsonPropertyName("activityId")]
    public string ActivityId { get; set; } = string.Empty;

    [JsonPropertyName("sessionEventsCount")]
    public int SessionEventsCount { get; set; } = 0;
}
