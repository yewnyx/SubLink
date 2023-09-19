using System;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace xyz.yewnyx.SubLink;

public sealed class Settings {
    [JsonPropertyName("Twitch"), ConfigurationKeyName("Twitch")]
    public TwitchSettings twitch { get; init; }

    [JsonPropertyName("Discord"), ConfigurationKeyName("Discord")]
    public DiscordSettings discord { get; init; }

    [JsonPropertyName("StreamPad"), ConfigurationKeyName("StreamPad")]
    public StreamPadSettings streampad { get; init; }

    [JsonPropertyName("SubLink"), ConfigurationKeyName("SubLink")]
    public SubLinkSettings sublink { get; init; }


    public Settings() {
        twitch = new();
        discord = new();
        streampad = new();
        sublink = new();
    }
    public Settings(TwitchSettings twitch, DiscordSettings discord, StreamPadSettings streampad, SubLinkSettings sublink) {
        this.twitch = twitch;
        this.discord = discord;
        this.streampad = streampad;
        this.sublink = sublink;
    }
}

public sealed class DiscordSettings {
    public ulong WebhookId => Convert.ToUInt64(Webhook.Split('/').SkipLast(1).Last());
    public string WebhookToken => Webhook.Split('/').Last();

    [JsonPropertyName("Webhook"), ConfigurationKeyName("Webhook")]
    public string Webhook { get; init; }

    public DiscordSettings() { }
    public DiscordSettings(string webhook) => Webhook = webhook;
}

public sealed class StreamPadSettings
{
    [JsonPropertyName("WebSocketUrl"), ConfigurationKeyName("WebSocketUrl")]
    public string WebSocketUrl { get; init; }

    [JsonPropertyName("ChannelId"), ConfigurationKeyName("ChannelId")]
    public string ChannelId { get; init; }

    public StreamPadSettings() { }

    public StreamPadSettings(string webSocketUrl, string channelId) =>
        (WebSocketUrl, ChannelId) = (webSocketUrl, channelId);
   
}
public sealed class SubLinkSettings {
    [JsonPropertyName("Discriminator"), ConfigurationKeyName("Discriminator")]
    public string Discriminator { get; init; }

    public SubLinkSettings() { }

    public SubLinkSettings(string discriminator) => Discriminator = discriminator;
}

public sealed class TwitchSettings {
    [JsonPropertyName("ClientId"), ConfigurationKeyName("ClientId")]
    public string ClientId { get; init; }

    [JsonPropertyName("ClientSecret"), ConfigurationKeyName("ClientSecret")]
    public string ClientSecret { get; init; }
    
    [JsonPropertyName("AccessToken"), ConfigurationKeyName("AccessToken")]
    public string AccessToken { get; init; }

    [JsonPropertyName("RefreshToken"), ConfigurationKeyName("RefreshToken")]
    public string RefreshToken { get; init; }

    public TwitchSettings() { }

    public TwitchSettings(string clientId, string clientSecret, string accessToken, string refreshToken) => 
        (ClientId, ClientSecret, AccessToken, RefreshToken) = (clientId, clientSecret, accessToken, refreshToken);
}