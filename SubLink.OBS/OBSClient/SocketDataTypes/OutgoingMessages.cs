using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

/**
 * Response to Hello message, should contain authentication string if authentication is required, along with PubSub subscriptions and other session parameters.
 */
internal class OutIdentifyMsg : IBaseMessage {
    public class Data {
        /** Version number that the client would like the obs-websocket server to use */
        [JsonPropertyName("rpcVersion")]
        public uint RpcVersion { get; set; } = 0;
        /** Authentication challenge response */
        [JsonPropertyName("authentication"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Authentication { get; set; }
        /** Bitmask of `EventSubscription` items to subscribe to events and event categories at will.
         * By default, all event categories are subscribed, except for events marked as high volume.
         * High volume events must be explicitly subscribed to. */
        [JsonPropertyName("eventSubscriptions")]
        public EventSubscription EventSubscriptions { get; set; } = EventSubscription.All;
    }

    [JsonPropertyName("d")]
    public Data D { get; set; } = new();

    public OutIdentifyMsg(uint rpcVersion) {
        D.RpcVersion = rpcVersion;
    }
}

/**
 * Sent at any time after initial identification to update the provided session parameters.
 */
internal class OutReidentifyMsg : IBaseMessage {
    /** Bitmask of `EventSubscription` items to subscribe to events and event categories at will.
     * By default, all event categories are subscribed, except for events marked as high volume.
     * High volume events must be explicitly subscribed to. */
    [JsonPropertyName("eventSubscriptions")]
    public EventSubscription EventSubscriptions { get; set; } = EventSubscription.All;
}

internal class OutRequestMsg : IBaseMessage
{
    [JsonPropertyName("requestType")]
    public int RequestType { get; set; } = -1;
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;
}

internal class OutRequestBatchMsg : IBaseMessage
{
}

