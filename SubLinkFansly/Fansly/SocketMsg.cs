using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class SocketMsg {
    [JsonPropertyName("t")]
    public uint Type { get; set; } = 0;

    [JsonPropertyName("d")]
    public string Data { get; set; } = string.Empty;

    public SocketMsg() { }

    public SocketMsg(uint type, string data) =>
        (Type, Data) = (type, data);
}
