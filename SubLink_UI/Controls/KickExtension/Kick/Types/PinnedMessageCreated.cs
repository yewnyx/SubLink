﻿using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

public sealed class PinnedMessageCreated {
    public sealed class PinnedMessageInfo {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("chatroom_id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public uint ChatroomId { get; set; } = 0;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonPropertyName("sender")]
        public KickUser Sender { get; set; } = new();

        [JsonPropertyName("metadata")]
        public object Metadata { get; set; } = new();
    }

    [JsonPropertyName("message")]
    public PinnedMessageInfo Message { get; set; } = new();

    [JsonPropertyName("duration")]
    public string Duration { get; set; } = string.Empty;
}
