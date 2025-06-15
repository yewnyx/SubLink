using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

internal enum OpCode : uint {
    /**
     * The initial message sent by obs-websocket to newly connected clients.
     * Initial OBS Version: 5.0.0
     */
    Hello = 0,
    /**
	 * The message sent by a newly connected client to obs-websocket in response to a `Hello`.
	 * Initial OBS Version: 5.0.0
	 */
    Identify = 1,
    /**
	 * The response sent by obs-websocket to a client after it has successfully identified with obs-websocket.
	 * Initial OBS Version: 5.0.0
	 */
    Identified = 2,
    /**
	 * The message sent by an already-identified client to update identification parameters.
	 * Initial OBS Version: 5.0.0
	 */
    Reidentify = 3,
    /**
	 * The message sent by obs-websocket containing an event payload.
	 * Initial OBS Version: 5.0.0
	 */
    Event = 5,
    /**
	 * The message sent by a client to obs-websocket to perform a request.
	 * Initial OBS Version: 5.0.0
	 */
    Request = 6,
    /**
	 * The message sent by obs-websocket in response to a particular request from a client.
	 * Initial OBS Version: 5.0.0
	 */
    RequestResponse = 7,
    /**
	 * The message sent by a client to obs-websocket to perform a batch of requests.
	 * Initial OBS Version: 5.0.0
	 */
    RequestBatch = 8,
    /**
	 * The message sent by obs-websocket in response to a particular batch of requests from a client.
	 * Initial OBS Version: 5.0.0
	 */
    RequestBatchResponse = 9,
}

// https://github.com/obs-websocket-community-projects/obs-websocket-js/blob/master/src/types.ts#L211
[JsonPolymorphic(
	UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor,
	TypeDiscriminatorPropertyName = "op"
)]
[JsonDerivedType(typeof(InHelloMsg), (int)OpCode.Hello)]
[JsonDerivedType(typeof(InIdentifiedMsg), (int)OpCode.Identified)]
[JsonDerivedType(typeof(InEventMsg), (int)OpCode.Event)]
[JsonDerivedType(typeof(InResponseMsg), (int)OpCode.RequestResponse)]
[JsonDerivedType(typeof(OutIdentifyMsg), (int)OpCode.Identify)]
[JsonDerivedType(typeof(OutReidentifyMsg), (int)OpCode.Reidentify)]
[JsonDerivedType(typeof(OutRequestMsg), (int)OpCode.Request)]
public interface IBaseMessage { }
