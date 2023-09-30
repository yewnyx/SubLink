using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Kick.Events;

public sealed class ChatroomUpdatedEvent {
    public sealed class SlowModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonPropertyName("message_interval")]
        public int MessageInterval { get; set; } = 0;
    }

    public sealed class SubsOnlyModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;
    }

    public sealed class FollowOnlyModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonPropertyName("min_duration")]
        public int MinDuration { get; set; } = 0;
    }

    public sealed class EmoteModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;
    }

    public sealed class AdvBotProtectModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonPropertyName("message_interval")]
        public int MessageInterval { get; set; } = 0;
    }

    [JsonPropertyName("id")]
    public uint Id { get; set; } = new();

    [JsonPropertyName("slow_mode")]
    public SlowModeInfo SlowMode { get; set; } = new();

    [JsonPropertyName("subscribers_mode")]
    public SubsOnlyModeInfo SubscribersMode { get; set; } = new();

    [JsonPropertyName("followers_mode")]
    public FollowOnlyModeInfo FollowersMode { get; set; } = new();

    [JsonPropertyName("emotes_mode")]
    public EmoteModeInfo EmotesMode { get; set; } = new();

    [JsonPropertyName("advanced_bot_protection")]
    public AdvBotProtectModeInfo AdvancedBotProtection { get; set; } = new();
}
