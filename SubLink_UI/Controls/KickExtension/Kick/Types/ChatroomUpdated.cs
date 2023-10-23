using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class ChatroomUpdated {
    public sealed class SlowModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonPropertyName("message_interval"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public uint MessageInterval { get; set; } = 0;
    }

    public sealed class SubsOnlyModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;
    }

    public sealed class FollowOnlyModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonPropertyName("min_duration"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public uint MinDuration { get; set; } = 0;
    }

    public sealed class EmoteModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;
    }

    public sealed class AdvBotProtectModeInfo {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonPropertyName("message_interval"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public uint MessageInterval { get; set; } = 0;
    }

    [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint ChatroomId { get; set; } = 0;

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
