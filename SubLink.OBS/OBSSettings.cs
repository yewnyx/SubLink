using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS;

public sealed class OBSSettings {
    [JsonPropertyName("Enabled"), ConfigurationKeyName("Enabled")]
    public bool Enabled { get; init; }

    [JsonPropertyName("ServerIp"), ConfigurationKeyName("ServerIp")]
    public string ServerIp { get; init; }

    [JsonPropertyName("ServerPort"), ConfigurationKeyName("ServerPort"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ushort ServerPort { get; init; }

    [JsonPropertyName("ServerPassword"), ConfigurationKeyName("ServerPassword")]
    public string ServerPassword { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public OBSSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public OBSSettings(string serverIp, ushort serverPort, string serverPassword) =>
        (ServerIp, ServerPort, ServerPassword) = (serverIp, serverPort, serverPassword);
}
