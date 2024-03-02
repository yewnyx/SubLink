using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.StreamElements;

public sealed class StreamElementsSettings {
    [JsonPropertyName("JWTToken"), ConfigurationKeyName("JWTToken")]
    public string JWTToken { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public StreamElementsSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public StreamElementsSettings(string jwtToken) =>
        JWTToken = jwtToken;
}
