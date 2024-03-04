using System.Text.Json;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.FanslyClient.SocketDataTypes;

internal sealed class SocketAuth {
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    public SocketAuth() { }

    public SocketAuth(string token) =>
        Token = token;

    public string ToSocketMsg() {
        SocketMsg msg = new(SocketMessageType.Auth, JsonSerializer.Serialize(this));
        return JsonSerializer.Serialize(msg);
    }
}
