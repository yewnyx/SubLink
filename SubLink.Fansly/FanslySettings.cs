using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly;

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