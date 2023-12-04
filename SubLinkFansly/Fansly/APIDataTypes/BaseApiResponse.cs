using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.APIDataTypes;

internal abstract class BaseApiResponse {
    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;
}
