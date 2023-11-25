using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class AccountInfoResponse {
    internal sealed class AccountInfo {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
    }

    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;

    [JsonPropertyName("response")]
    public AccountInfo[] Response { get; set; } = Array.Empty<AccountInfo>();
}
