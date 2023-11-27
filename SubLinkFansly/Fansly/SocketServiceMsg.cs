using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class SocketServiceMsg {
    [JsonPropertyName("serviceId")]
    public uint ServiceId { get; set; } = 0;

    [JsonPropertyName("event")]
    public string Event { get; set; } = string.Empty;

    public SocketServiceMsg() { }

    public SocketServiceMsg(uint serviceId, string event_) =>
        (ServiceId, Event) = (serviceId, event_);
}
