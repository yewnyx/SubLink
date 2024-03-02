using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink;

internal sealed class Settings {
#pragma warning disable IDE1006 // Naming Styles
    [JsonPropertyName("Discord"), ConfigurationKeyName("Discord")]
    public DiscordSettings discord { get; init; }

    [JsonPropertyName("SubLink"), ConfigurationKeyName("SubLink")]
    public SubLinkSettings sublink { get; init; }
#pragma warning restore IDE1006 // Naming Styles

    public Settings() {
        discord = new();
        sublink = new();
    }

    public Settings(DiscordSettings discord, SubLinkSettings sublink) {
        this.discord = discord;
        this.sublink = sublink;
    }
}

internal sealed class DiscordSettings {
    public ulong WebhookId => Convert.ToUInt64(Webhook.Split('/').SkipLast(1).Last());
    public string WebhookToken => Webhook.Split('/').Last();

    [JsonPropertyName("Webhook"), ConfigurationKeyName("Webhook")]
    public string Webhook { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DiscordSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DiscordSettings(string webhook) => Webhook = webhook;
}


internal sealed class SubLinkSettings {
    [JsonPropertyName("Discriminator"), ConfigurationKeyName("Discriminator")]
    public string Discriminator { get; init; }

    [JsonPropertyName("OscIPAddress"), ConfigurationKeyName("OscIPAddress")]
    public string OscIPAddress { get; init; }

    [JsonPropertyName("OscPort"), ConfigurationKeyName("OscPort")]
    public int OscPort { get; init; }

    [JsonPropertyName("ScriptName"), ConfigurationKeyName("ScriptName")]
    public string ScriptName { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SubLinkSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public SubLinkSettings(string discriminator, string oscIPAddress, int oscPort, string scriptName) =>
        (Discriminator, OscIPAddress, OscPort, ScriptName) = (discriminator, oscIPAddress, oscPort, scriptName);
}
