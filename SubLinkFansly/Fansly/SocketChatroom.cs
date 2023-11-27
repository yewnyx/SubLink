using System.Text.Json;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class SocketChatroom {
    [JsonPropertyName("chatRoomId")]
    public string ChatRoomId { get; set; } = string.Empty;

    public SocketChatroom() { }

    public SocketChatroom(string chatRoomId) =>
        (ChatRoomId) = (chatRoomId);

    public string ToSocketMsg() {
        SocketMsg msg = new(46001, JsonSerializer.Serialize(this));
        return JsonSerializer.Serialize(msg);
    }
}
