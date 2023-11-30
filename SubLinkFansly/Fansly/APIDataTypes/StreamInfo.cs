using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.APIDataTypes;

internal sealed class StreamInfoResponse {
    internal sealed class StreamInfo {
        internal sealed class StreamData {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("title")]
            public string Title { get; set; } = string.Empty;

            [JsonPropertyName("status")]
            public uint Status { get; set; } = 0;

            [JsonPropertyName("viewerCount")]
            public uint ViewerCount { get; set; } = 0;

            [JsonPropertyName("lastFetchedAt")]
            public ulong LastFetchedAt { get; set; } = 0;

            [JsonPropertyName("startedAt")]
            public ulong StartedAt { get; set; } = 0;

            [JsonPropertyName("playbackUrl")]
            public string PlaybackUrl { get; set; } = string.Empty;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = string.Empty;

        [JsonPropertyName("playbackUrl")]
        public string PlaybackUrl { get; set; } = string.Empty;

        [JsonPropertyName("chatRoomId")]
        public string ChatRoomId { get; set; } = string.Empty;

        [JsonPropertyName("stream")]
        public StreamData Stream { get; set; } = new();
    }

    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;

    [JsonPropertyName("response")]
    public StreamInfo Response { get; set; } = new();
}
