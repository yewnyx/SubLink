using Newtonsoft.Json;
using System;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class PollUpdateEvent {
    public sealed class PollOptionInfo {
        [JsonProperty("id")]
        public uint Id { get; set; } = 0;

        [JsonProperty("label")]
        public string Label { get; set; } = string.Empty;

        [JsonProperty("votes")]
        public int Votes { get; set; } = 0;
    }

    public sealed class PollInfo {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("options")]
        public PollOptionInfo[] Options { get; set; } = Array.Empty<PollOptionInfo>();

        [JsonProperty("duration")]
        public int Duration { get; set; } = 0;

        [JsonProperty("remaining")]
        public int Remaining { get; set; } = 0;

        [JsonProperty("result_display_duration")]
        public int ResultDisplayDuration { get; set; } = 0;
    }
}
