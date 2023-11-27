using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.EventTypes;

internal sealed class TipMessageMetadata {
    [JsonPropertyName("amount")]
    public uint Amount { get; set; } = 0;

    public TipMessageMetadata() { }

    public TipMessageMetadata(uint amount) =>
        Amount = amount;
}
