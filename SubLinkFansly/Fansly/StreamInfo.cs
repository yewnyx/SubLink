using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class StreamInfoResponse {
    internal sealed class StreamInfo {
        internal sealed class StreamData {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("title")]
            public string Title { get; set; } = string.Empty;

            [JsonPropertyName("status")]
            public string Status { get; set; } = string.Empty;

            [JsonPropertyName("lastFetchedAt")]
            public string LastFetchedAt { get; set; } = string.Empty;

            [JsonPropertyName("startedAt")]
            public string StartedAt { get; set; } = string.Empty;

            [JsonPropertyName("playbackUrl")]
            public string PlaybackUrl { get; set; } = string.Empty;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = string.Empty;

        [JsonPropertyName("playbackUrl")]
        public string PlaybackUrl { get; set; } = string.Empty;

        [JsonPropertyName("stream")]
        public StreamData Stream { get; set; } = new();
    }

    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;

    [JsonPropertyName("response")]
    public StreamInfo[] Response { get; set; } = Array.Empty<StreamInfo>();
}
