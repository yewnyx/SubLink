using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class PollUpdateEvent {
    public sealed class PollOptionInfo {
        [JsonPropertyName("id")]
        public uint Id { get; set; } = 0;

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("votes")]
        public int Votes { get; set; } = 0;
    }

    public sealed class PollInfo {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public PollOptionInfo[] Options { get; set; } = Array.Empty<PollOptionInfo>();

        [JsonPropertyName("duration")]
        public int Duration { get; set; } = 0;

        [JsonPropertyName("remaining")]
        public int Remaining { get; set; } = 0;

        [JsonPropertyName("result_display_duration")]
        public int ResultDisplayDuration { get; set; } = 0;
    }

    [JsonPropertyName("poll")]
    public PollInfo Poll { get; set; } = new();
}
