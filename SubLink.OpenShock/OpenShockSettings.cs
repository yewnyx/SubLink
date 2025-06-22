using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OpenShock;

public sealed class OpenShockSettings {
    [JsonPropertyName("Enabled"), ConfigurationKeyName("Enabled")]
    public bool Enabled { get; init; }
    [JsonPropertyName("Server"), ConfigurationKeyName("Server")]
    public string Server { get; init; }

    [JsonPropertyName("Token"), ConfigurationKeyName("Token")]
    public string Token { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public OpenShockSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public OpenShockSettings(bool enabled, string server, string token) =>
        (Enabled, Server, Token) = (enabled, server, token);
}