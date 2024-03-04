using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.Fansly.FanslyClient.SocketDataTypes;

internal sealed class SocketService {
    [JsonPropertyName("serviceId")]
    public uint ServiceId { get; set; } = 0;

    [JsonPropertyName("event")]
    public string Event { get; set; } = string.Empty;

    public SocketService() { }

    public SocketService(uint serviceId, string event_) =>
        (ServiceId, Event) = (serviceId, event_);
}
