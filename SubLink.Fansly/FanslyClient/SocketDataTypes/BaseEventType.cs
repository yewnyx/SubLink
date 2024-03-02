using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.FanslyClient.SocketDataTypes;

internal enum EventType : uint {
    Unknown         =  0,
    ChatRoomMessage = 10,
    ChatRoomGoal    = 51
}

internal class BaseEventType {
    [JsonPropertyName("type")]
    public EventType Type { get; set; } = EventType.Unknown;

    public BaseEventType() { }

    public BaseEventType(EventType type) =>
        Type = type;
}
