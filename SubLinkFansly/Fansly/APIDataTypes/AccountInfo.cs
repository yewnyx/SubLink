using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.APIDataTypes;

internal sealed class AccountInfoResponse : BaseApiResponse {
    internal sealed class AccountInfo {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
    }

    [JsonPropertyName("response")]
    public AccountInfo[] Response { get; set; } = Array.Empty<AccountInfo>();
}
