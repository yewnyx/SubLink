using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Discord;

public sealed class DiscordSettings {
    [JsonPropertyName("Enabled"), ConfigurationKeyName("Enabled")]
    public bool Enabled { get; init; }
    [JsonPropertyName("ClientID"), ConfigurationKeyName("ClientID")]
    public string ClientID { get; init; }
    [JsonPropertyName("ClientSecret"), ConfigurationKeyName("ClientSecret")]
    public string ClientSecret { get; init; }
    [JsonPropertyName("DefaultGuildId"), ConfigurationKeyName("DefaultGuildId")]
    public string DefaultGuildId { get; init; }
    [JsonPropertyName("DefaultChannelId"), ConfigurationKeyName("DefaultChannelId")]
    public string DefaultChannelId { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DiscordSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DiscordSettings(bool enabled, string clientID, string clientSecret, string defaultGuildId, string defaultChannelId) =>
        (Enabled, ClientID, ClientSecret, DefaultGuildId, DefaultChannelId) = (enabled, clientID, clientSecret, defaultGuildId, defaultChannelId);
}