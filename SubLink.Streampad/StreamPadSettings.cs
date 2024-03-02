using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Streampad;

public sealed class StreamPadSettings {
    [JsonPropertyName("WebSocketUrl"), ConfigurationKeyName("WebSocketUrl")]
    public string WebSocketUrl { get; init; }

    [JsonPropertyName("ChannelId"), ConfigurationKeyName("ChannelId")]
    public string ChannelId { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public StreamPadSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public StreamPadSettings(string webSocketUrl, string channelId) =>
        (WebSocketUrl, ChannelId) = (webSocketUrl, channelId);
}
