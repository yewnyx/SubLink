using Newtonsoft.Json;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class ChatroomUpdatedEvent {
    public sealed class SlowModeInfo {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonProperty("message_interval")]
        public int MessageInterval { get; set; } = 0;
    }

    public sealed class SubsOnlyModeInfo {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = false;
    }

    public sealed class FollowOnlyModeInfo {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonProperty("min_duration")]
        public int MinDuration { get; set; } = 0;
    }

    public sealed class EmoteModeInfo {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = false;
    }

    public sealed class AdvBotProtectModeInfo {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonProperty("message_interval")]
        public int MessageInterval { get; set; } = 0;
    }

    [JsonProperty("id")]
    public uint Id { get; set; } = new();

    [JsonProperty("slow_mode")]
    public SlowModeInfo SlowMode { get; set; } = new();

    [JsonProperty("subscribers_mode")]
    public SubsOnlyModeInfo SubscribersMode { get; set; } = new();

    [JsonProperty("followers_mode")]
    public FollowOnlyModeInfo FollowersMode { get; set; } = new();

    [JsonProperty("emotes_mode")]
    public EmoteModeInfo EmotesMode { get; set; } = new();

    [JsonProperty("advanced_bot_protection")]
    public AdvBotProtectModeInfo AdvancedBotProtection { get; set; } = new();
}
