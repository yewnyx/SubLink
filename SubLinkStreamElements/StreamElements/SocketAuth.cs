using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.StreamElements;

internal sealed class SocketAuth {
    [JsonPropertyName("method")]
    public string Method { get; set; } = string.Empty;

    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    public SocketAuth() { }

    public SocketAuth(string method, string token) {
        Method = method;
        Token = token;
    }
}
