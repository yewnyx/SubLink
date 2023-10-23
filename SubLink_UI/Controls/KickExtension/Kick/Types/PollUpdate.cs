using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class PollUpdate {
    public sealed class PollInfo {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public PollOptionInfo[] Options { get; set; } = Array.Empty<PollOptionInfo>();

        [JsonPropertyName("duration"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public uint Duration { get; set; } = 0;

        [JsonPropertyName("remaining"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int Remaining { get; set; } = 0;

        [JsonPropertyName("result_display_duration"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public uint ResultDisplayDuration { get; set; } = 0;
    }

    [JsonPropertyName("poll")]
    public PollInfo Poll { get; set; } = new();
}
