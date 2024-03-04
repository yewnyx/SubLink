using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Twitch;

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
