using System;
using System.Text.Json.Serialization;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Request;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

/**
 * Response to Hello message, should contain authentication string if authentication is required, along with PubSub subscriptions and other session parameters.
 */
[JsonPolymorphic(
	UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor,
	TypeDiscriminatorPropertyName = "op"
)]
[JsonDerivedType(typeof(OutIdentifyMsg), (int)OpCode.Identify)]
internal class OutIdentifyMsg : IBaseMessage {
    public class Data {
        /** Version number that the client would like the obs-websocket server to use */
        [JsonPropertyName("rpcVersion")]
        public uint RpcVersion { get; set; } = 0;
        /** Authentication challenge response */
        [JsonPropertyName("authentication")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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
[JsonPolymorphic(
	UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor,
	TypeDiscriminatorPropertyName = "op"
)]
[JsonDerivedType(typeof(OutReidentifyMsg), (int)OpCode.Reidentify)]
internal class OutReidentifyMsg : IBaseMessage {
    public class Data {
        /** Bitmask of `EventSubscription` items to subscribe to events and event categories at will.
         * By default, all event categories are subscribed, except for events marked as high volume.
         * High volume events must be explicitly subscribed to. */
        [JsonPropertyName("eventSubscriptions")]
        public EventSubscription EventSubscriptions { get; set; } = EventSubscription.All;
    }

    [JsonPropertyName("d")]
    public Data D { get; set; } = new();
}

[JsonPolymorphic(
	UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor,
	TypeDiscriminatorPropertyName = "op"
)]
[JsonDerivedType(typeof(OutRequestMsg), (int)OpCode.Request)]
internal class OutRequestMsg : IBaseMessage {
    public class Data {
        [JsonPropertyName("requestId")]
        public string RequestId { get; init; } = Guid.NewGuid().ToString();
        [JsonPropertyName("requestType")]
        public string RequestType { get; set; } = string.Empty;
        [JsonPropertyName("requestData")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IRequestDataType? RequestData { get; set; }
    }

    [JsonPropertyName("d")]
    public Data D { get; set; } = new();
}

