using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.FanslyClient.APIDataTypes;

internal abstract class BaseApiResponse {
    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;
}
