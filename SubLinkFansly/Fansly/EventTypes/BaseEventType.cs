using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.EventTypes;

internal class BaseEventType {
    [JsonPropertyName("type")]
    public uint Type { get; set; } = 0;

    public BaseEventType() { }

    public BaseEventType(uint type) =>
        Type = type;
}
