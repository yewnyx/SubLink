using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class StreamHost {
    [JsonPropertyName("chatroom_id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint ChatroomId { get; set; } = 0;

    [JsonPropertyName("optional_message")]
    public string OptionalMessage { get; set; } = string.Empty;

    [JsonPropertyName("number_viewers"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint NumberViewers { get; set; } = 0;

    [JsonPropertyName("host_username")]
    public string HostUsername { get; set; } = string.Empty;
}
