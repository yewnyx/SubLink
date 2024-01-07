using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace xyz.yewnyx.SubLink;

public sealed class Settings {
    [JsonPropertyName("Twitch"), ConfigurationKeyName("Twitch")]
    public TwitchSettings twitch { get; init; }

    [JsonPropertyName("Kick"), ConfigurationKeyName("Kick")]
    public KickSettings kick { get; init; }

    [JsonPropertyName("StreamPad"), ConfigurationKeyName("StreamPad")]
    public StreamPadSettings streampad { get; init; }

    [JsonPropertyName("StreamElements"), ConfigurationKeyName("StreamElements")]
    public StreamElementsSettings streamElements { get; init; }

    [JsonPropertyName("Fansly"), ConfigurationKeyName("Fansly")]
    public FanslySettings fansly { get; init; }

    [JsonPropertyName("Discord"), ConfigurationKeyName("Discord")]
    public DiscordSettings discord { get; init; }

    [JsonPropertyName("SubLink"), ConfigurationKeyName("SubLink")]
    public SubLinkSettings sublink { get; init; }


    public Settings() {
        twitch = new();
        kick = new();
        streampad = new();
        streamElements = new();
        fansly = new();
        discord = new();
        sublink = new();
    }
    public Settings(TwitchSettings twitch, KickSettings kick, StreamPadSettings streampad,
        StreamElementsSettings streamElements, FanslySettings fansly,
        DiscordSettings discord, SubLinkSettings sublink) {
        this.twitch = twitch;
        this.kick = kick;
        this.streampad = streampad;
        this.streamElements = streamElements;
        this.fansly = fansly;
        this.discord = discord;
        this.sublink = sublink;
    }
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TwitchSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public TwitchSettings(string clientId, string clientSecret, string accessToken, string refreshToken) =>
        (ClientId, ClientSecret, AccessToken, RefreshToken) = (clientId, clientSecret, accessToken, refreshToken);
}

public sealed class KickSettings {
    [JsonPropertyName("PusherKey"), ConfigurationKeyName("PusherKey")]
    public string PusherKey { get; init; }

    [JsonPropertyName("PusherCluster"), ConfigurationKeyName("PusherCluster")]
    public string PusherCluster { get; init; }

    [JsonPropertyName("ChatroomId"), ConfigurationKeyName("ChatroomId")]
    public string ChatroomId { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public KickSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public KickSettings(string pusherKey, string pusherCluster, string chatroomId) =>
        (PusherKey, PusherCluster, ChatroomId) = (pusherKey, pusherCluster, chatroomId);
}

public sealed class StreamPadSettings {
    [JsonPropertyName("WebSocketUrl"), ConfigurationKeyName("WebSocketUrl")]
    public string WebSocketUrl { get; init; }

    [JsonPropertyName("ChannelId"), ConfigurationKeyName("ChannelId")]
    public string ChannelId { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public StreamPadSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public StreamPadSettings(string webSocketUrl, string channelId) =>
        (WebSocketUrl, ChannelId) = (webSocketUrl, channelId);

}

public sealed class StreamElementsSettings {
    [JsonPropertyName("JWTToken"), ConfigurationKeyName("JWTToken")]
    public string JWTToken { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public StreamElementsSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public StreamElementsSettings(string jwtToken) =>
        JWTToken = jwtToken;
}

public sealed class FanslySettings {
    [JsonPropertyName("Token"), ConfigurationKeyName("Token")]
    public string Token { get; init; }

    [JsonPropertyName("Username"), ConfigurationKeyName("Username")]
    public string Username { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public FanslySettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public FanslySettings(string token, string username) =>
        (Token, Username) = (token, username);
}

public sealed class DiscordSettings {
    public ulong WebhookId => Convert.ToUInt64(Webhook.Split('/').SkipLast(1).Last());
    public string WebhookToken => Webhook.Split('/').Last();

    [JsonPropertyName("Webhook"), ConfigurationKeyName("Webhook")]
    public string Webhook { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DiscordSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DiscordSettings(string webhook) => Webhook = webhook;
}


public sealed class SubLinkSettings {
    [JsonPropertyName("Discriminator"), ConfigurationKeyName("Discriminator")]
    public string Discriminator { get; init; }

    [JsonPropertyName("OscIPAddress"), ConfigurationKeyName("OscIPAddress")]
    public string OscIPAddress { get; init; } = "127.0.0.1";

    [JsonPropertyName("OscPort"), ConfigurationKeyName("OscPort")]
    public int OscPort { get; init; } = 9000;

    [JsonPropertyName("ScriptName"), ConfigurationKeyName("ScriptName")]
    public string ScriptName { get; init; } = "SubLink.cs";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SubLinkSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public SubLinkSettings(string discriminator, string oscIPAddress, int oscPort, string scriptName) =>
        (Discriminator, OscIPAddress, OscPort, ScriptName) = (discriminator, oscIPAddress, oscPort, scriptName);
}
