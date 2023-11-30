using System;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.SocketDataTypes;

internal enum AttachmentContentType : uint {
    Unknown = 0,
    Tip     = 7
}

internal sealed class ChatRoomMessage : BaseEventType {
    public sealed class InnerData {
        public sealed class AttachmentInfo {
            [JsonPropertyName("contentType")]
            public AttachmentContentType ContentType { get; set; } = 0;

            [JsonPropertyName("contentId")]
            public string ContentId { get; set; } = string.Empty;

            [JsonPropertyName("metadata")]
            public string Metadata { get; set; } = string.Empty;

            [JsonPropertyName("chatRoomMessageId")]
            public string ChatRoomMessageId { get; set; } = string.Empty;
        }

        [JsonPropertyName("chatRoomId")]
        public string ChatRoomId { get; set; } = string.Empty;

        [JsonPropertyName("senderId")]
        public string SenderId { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public uint Type { get; set; } = 0;

        [JsonPropertyName("attachments")]
        public AttachmentInfo[] Attachments { get; set; } = Array.Empty<AttachmentInfo>();

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public long CreatedAt { get; set; } = 0;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("displayname")]
        public string Displayname { get; set; } = string.Empty;
    }

    [JsonPropertyName("chatRoomMessage")]
    public InnerData Data { get; set; } = new();

    public ChatRoomMessage() { }
}
