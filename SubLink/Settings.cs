using System;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace xyz.yewnyx.SubLink;

public sealed class Settings {
    [JsonPropertyName("Kick"), ConfigurationKeyName("Kick")]
    public KickSettings kick { get; init; }

    [JsonPropertyName("Discord"), ConfigurationKeyName("Discord")]
    public DiscordSettings discord { get; init; }

    [JsonPropertyName("SubLink"), ConfigurationKeyName("SubLink")]
    public SubLinkSettings sublink { get; init; }


    public Settings() {
        kick = new();
        discord = new();
        sublink = new();
    }
    public Settings(KickSettings kick, DiscordSettings discord, SubLinkSettings sublink) {
        this.kick = kick;
        this.discord = discord;
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


public sealed class SubLinkSettings {
    [JsonPropertyName("Discriminator"), ConfigurationKeyName("Discriminator")]
    public string Discriminator { get; init; }

    public SubLinkSettings() { }

    public SubLinkSettings(string discriminator) => Discriminator = discriminator;
}

public sealed class KickSettings {
    [JsonPropertyName("PusherKey"), ConfigurationKeyName("PusherKey")]
    public string PusherKey { get; init; }

    [JsonPropertyName("PusherCluster"), ConfigurationKeyName("PusherCluster")]
    public string PusherCluster { get; init; }
    
    [JsonPropertyName("ChatroomId"), ConfigurationKeyName("ChatroomId")]
    public string ChatroomId { get; init; }

    public KickSettings() { }

    public KickSettings(string pusherKey, string pusherCluster, string chatroomId) =>
        (PusherKey, PusherCluster, ChatroomId) = (pusherKey, pusherCluster, chatroomId);
}