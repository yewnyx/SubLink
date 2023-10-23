using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class Subscription {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("chatroom_id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint ChatroomId { get; set; } = 0;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("months"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Months { get; set; } = 0;
}
