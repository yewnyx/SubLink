using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly;

internal enum SocketMessageType : uint {
    Unknown  =     0,
    Auth     =     1,
    Service  = 10000,
    ChatRoom = 46001
}

internal sealed class SocketMsg {
    [JsonPropertyName("t")]
    public SocketMessageType Type { get; set; } = SocketMessageType.Unknown;

    [JsonPropertyName("d")]
    public string Data { get; set; } = string.Empty;

    public SocketMsg() { }

    public SocketMsg(SocketMessageType type, string data) =>
        (Type, Data) = (type, data);
}
