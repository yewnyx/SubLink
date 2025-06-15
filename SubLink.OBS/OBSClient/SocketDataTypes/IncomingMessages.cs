using System.Text.Json.Serialization;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Event;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Response;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

/**
 * Message sent from the server immediately on client connection.
 * Contains authentication information if auth is required. Also contains RPC version for version negotiation.
 */
internal class InHelloMsg : IBaseMessage {
    public class DataAuth {
        [JsonPropertyName("challenge")]
        public string Challenge { get; set; } = string.Empty;
        [JsonPropertyName("salt")]
        public string Salt { get; set; } = string.Empty;
    }

    public class Data {
        /** Version number of obs-websocket */
        [JsonPropertyName("obsWebSocketVersion")]
        public string ObsWebSocketVersion { get; set; } = string.Empty;
        /** Version number which gets incremented on each breaking change to the obs-websocket protocol.
		 * It's usage in this context is to provide the current rpc version that the server would like to use. */
        [JsonPropertyName("rpcVersion")]
        public uint RpcVersion { get; set; } = 0;
        /** Authentication challenge when password is required */
        [JsonPropertyName("authentication")]
        public DataAuth? Authentication { get; set; }
    }

    [JsonPropertyName("d")]
    public Data D { get; set; } = new();
}

/**
 * The identify request was received and validated, and the connection is now ready for normal operation.
 */
internal class InIdentifiedMsg : IBaseMessage {
    public class Data {
        /** If rpc version negotiation succeeds, the server determines the RPC version to be used and gives it to the client */
        [JsonPropertyName("negotiatedRpcVersion")]
        public uint NegotiatedRpcVersion { get; set; } = 0;
    }

    [JsonPropertyName("d")]
    public Data D { get; set; } = new();
}

/**
 * An event coming from OBS has occured. Eg scene switched, source muted.
 */
public class InEventMsg : IBaseMessage {
    [JsonPropertyName("d")]
    [JsonConverter(typeof(EventTypeConverter))]
    public EventDataType? D { get; set; }
}

/**
 * obs-websocket is responding to a request coming from a client
 */
public class InResponseMsg : IBaseMessage {
    public class RequestStatusObj {
        [JsonPropertyName("result")]
        public bool Result { get; set; } = false;
        [JsonPropertyName("code")]
        public int Code { get; set; } = -1;
        [JsonPropertyName("comment")]
        public string? Comment { get; set; }
    }

    public class Data {
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; } = string.Empty;
        [JsonPropertyName("requestStatus")]
        public RequestStatusObj RequestStatus { get; set; } = new();
        [JsonPropertyName("requestType")]
        public string RequestType { get; set; } = string.Empty;
        [JsonPropertyName("responseData")]
        public IResponseType? ResponseData { get; set; }
    }

    [JsonPropertyName("d")]
    [JsonConverter(typeof(DataTypeConverter))]
    public Data D { get; set; } = new();
}
