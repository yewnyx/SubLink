using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

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
