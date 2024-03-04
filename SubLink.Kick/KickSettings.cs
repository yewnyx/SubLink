using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick;

public sealed class KickSettings {
    [JsonPropertyName("PusherKey"), ConfigurationKeyName("PusherKey")]
    public string PusherKey { get; init; }

    [JsonPropertyName("PusherCluster"), ConfigurationKeyName("PusherCluster")]
    public string PusherCluster { get; init; }

    [JsonPropertyName("ChatroomId"), ConfigurationKeyName("ChatroomId")]
    public string ChatroomId { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public KickSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public KickSettings(string pusherKey, string pusherCluster, string chatroomId) =>
        (PusherKey, PusherCluster, ChatroomId) = (pusherKey, pusherCluster, chatroomId);
}
