using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

public enum OpCode : uint {
    /**
     * The initial message sent by obs-websocket to newly connected clients.
     * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
     */
    Hello = 0,
    /**
	 * The message sent by a newly connected client to obs-websocket in response to a `Hello`.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Identify = 1,
    /**
	 * The response sent by obs-websocket to a client after it has successfully identified with obs-websocket.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Identified = 2,
    /**
	 * The message sent by an already-identified client to update identification parameters.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Reidentify = 3,
    /**
	 * The message sent by obs-websocket containing an event payload.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Event = 5,
    /**
	 * The message sent by a client to obs-websocket to perform a request.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Request = 6,
    /**
	 * The message sent by obs-websocket in response to a particular request from a client.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    RequestResponse = 7,
    /**
	 * The message sent by a client to obs-websocket to perform a batch of requests.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    RequestBatch = 8,
    /**
	 * The message sent by obs-websocket in response to a particular batch of requests from a client.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    RequestBatchResponse = 9,
}

public enum WebSocketCloseCode: uint {
    /**
	 * For internal use only to tell the request handler not to perform any close action.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    DontClose = 0,
    /**
	 * Unknown reason, should never be used.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    UnknownReason = 4000,
    /**
	 * The server was unable to decode the incoming websocket message.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    MessageDecodeError = 4002,
    /**
	 * A data field is required but missing from the payload.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    MissingDataField = 4003,
    /**
	 * A data field's value type is invalid.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidDataFieldType = 4004,
    /**
	 * A data field's value is invalid.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidDataFieldValue = 4005,
    /**
	 * The specified `op` was invalid or missing.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    UnknownOpCode = 4006,
    /**
	 * The client sent a websocket message without first sending `Identify` message.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    NotIdentified = 4007,
    /**
	 * The client sent an `Identify` message while already identified.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    AlreadyIdentified = 4008,
    /**
	 * The authentication attempt (via `Identify`) failed.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    AuthenticationFailed = 4009,
    /**
	 * The server detected the usage of an old version of the obs-websocket RPC protocol.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    UnsupportedRpcVersion = 4010,
    /**
	 * The websocket session has been invalidated by the obs-websocket server.
	 * Note: This is the code used by the `Kick` button in the UI Session List. If you receive this code, you must not automatically reconnect.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    SessionInvalidated = 4011,
    /**
	 * A requested feature is not supported due to hardware/software limitations.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    UnsupportedFeature = 4012,
}

public enum RequestBatchExecutionType: int {
    /**
	 * Not a request batch.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    None = -1,
    /**
	 * A request batch which processes all requests serially, as fast as possible.
	 * Note: To introduce artificial delay, use the `Sleep` request and the `sleepMillis` request field.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    SerialRealtime = 0,
    /**
	 * A request batch type which processes all requests serially, in sync with the graphics thread. Designed to provide high accuracy for animations.
	 * Note: To introduce artificial delay, use the `Sleep` request and the `sleepMillis` request field.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    SerialFrame = 1,
    /**
	 * A request batch type which processes all requests using all available threads in the thread pool.
	 * Note: This is mainly experimental, and only really shows its colors during requests which require lots of active processing, like `GetSourceScreenshot`.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Parallel = 2,
}

public enum RequestStatus: uint {
    /**
	 * Unknown status, should never be used.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Unknown = 0,
    /**
	 * For internal use to signify a successful field check.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    NoError = 10,
    /**
	 * The request has succeeded.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Success = 100,
    /**
	 * The `requestType` field is missing from the request data.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    MissingRequestType = 203,
    /**
	 * The request type is invalid or does not exist.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    UnknownRequestType = 204,
    /**
	 * Generic error code.
	 * Note: A comment is required to be provided by obs-websocket.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    GenericError = 205,
    /**
	 * The request batch execution type is not supported.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    UnsupportedRequestBatchExecutionType = 206,
    /**
	 * The server is not ready to handle the request.
	 * Note: This usually occurs during OBS scene collection change or exit. Requests may be tried again after a delay if this code is given.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    NotReady = 207,
    /**
	 * A required request field is missing.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    MissingRequestField = 300,
    /**
	 * The request does not have a valid requestData object.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    MissingRequestData = 301,
    /**
	 * Generic invalid request field message.
	 * Note: A comment is required to be provided by obs-websocket.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidRequestField = 400,
    /**
	 * A request field has the wrong data type.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidRequestFieldType = 401,
    /**
	 * A request field (number) is outside of the allowed range.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    RequestFieldOutOfRange = 402,
    /**
	 * A request field (string or array) is empty and cannot be.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    RequestFieldEmpty = 403,
    /**
	 * There are too many request fields (eg. a request takes two optionals, where only one is allowed at a time).
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    TooManyRequestFields = 404,
    /**
	 * An output is running and cannot be in order to perform the request.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OutputRunning = 500,
    /**
	 * An output is not running and should be.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OutputNotRunning = 501,
    /**
	 * An output is paused and should not be.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OutputPaused = 502,
    /**
	 * An output is not paused and should be.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OutputNotPaused = 503,
    /**
	 * An output is disabled and should not be.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OutputDisabled = 504,
    /**
	 * Studio mode is active and cannot be.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    StudioModeActive = 505,
    /**
	 * Studio mode is not active and should be.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    StudioModeNotActive = 506,
    /**
	 * The resource was not found.
	 * Note: Resources are any kind of object in obs-websocket, like inputs, profiles, outputs, etc.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    ResourceNotFound = 600,
    /**
	 * The resource already exists.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    ResourceAlreadyExists = 601,
    /**
	 * The type of resource found is invalid.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidResourceType = 602,
    /**
	 * There are not enough instances of the resource in order to perform the request.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    NotEnoughResources = 603,
    /**
	 * The state of the resource is invalid. For example, if the resource is blocked from being accessed.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidResourceState = 604,
    /**
	 * The specified input (obs_source_t-OBS_SOURCE_TYPE_INPUT) had the wrong kind.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidInputKind = 605,
    /**
	 * The resource does not support being configured.
	 * This is particularly relevant to transitions, where they do not always have changeable settings.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    ResourceNotConfigurable = 606,
    /**
	 * The specified filter (obs_source_t-OBS_SOURCE_TYPE_FILTER) had the wrong kind.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InvalidFilterKind = 607,
    /**
	 * Creating the resource failed.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    ResourceCreationFailed = 700,
    /**
	 * Performing an action on the resource failed.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    ResourceActionFailed = 701,
    /**
	 * Processing the request failed unexpectedly.
	 * Note: A comment is required to be provided by obs-websocket.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    RequestProcessingFailed = 702,
    /**
	 * The combination of request fields cannot be used to perform an action.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    CannotAct = 703,
}

public enum EventSubscription: uint {
    /**
	 * Subcription value used to disable all events.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    None = 0,
    /**
	 * Subscription value to receive events in the `General` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    General = 1 << 0,
    /**
	 * Subscription value to receive events in the `Config` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Config = 1 << 1,
    /**
	 * Subscription value to receive events in the `Scenes` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Scenes = 1 << 2,
    /**
	 * Subscription value to receive events in the `Inputs` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Inputs = 1 << 3,
    /**
	 * Subscription value to receive events in the `Transitions` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Transitions = 1 << 4,
    /**
	 * Subscription value to receive events in the `Filters` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Filters = 1 << 5,
    /**
	 * Subscription value to receive events in the `Outputs` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Outputs = 1 << 6,
    /**
	 * Subscription value to receive events in the `SceneItems` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    SceneItems = 1 << 7,
    /**
	 * Subscription value to receive events in the `MediaInputs` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    MediaInputs = 1 << 8,
    /**
	 * Subscription value to receive events in the `VendorEvent` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Vendors = 1 << 9,
    /**
	 * Subscription value to receive events in the `Ui` category.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    Ui = 1 << 10,
    /**
	 * Helper to receive all non-high-volume events.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    All = General | Config | Scenes | Inputs | Transitions | Filters | Outputs | SceneItems | MediaInputs | Vendors | Ui,
    /**
	 * Subscription value to receive the `InputVolumeMeters` high-volume event.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InputVolumeMeters = 1 << 16,
    /**
	 * Subscription value to receive the `InputActiveStateChanged` high-volume event.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InputActiveStateChanged = 1 << 17,
    /**
	 * Subscription value to receive the `InputShowStateChanged` high-volume event.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    InputShowStateChanged = 1 << 18,
    /**
	 * Subscription value to receive the `SceneItemTransformChanged` high-volume event.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    SceneItemTransformChanged = 1 << 19,
}

[JsonConverter(typeof(ObsMediaInputActionConverter))]
public enum ObsMediaInputAction
{
    /**
	 * No action.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NONE,
    /**
	 * Play the media input.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PLAY,
    /**
	 * Pause the media input.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PAUSE,
    /**
	 * Stop the media input.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_MEDIA_INPUT_ACTION_STOP,
    /**
	 * Restart the media input.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_MEDIA_INPUT_ACTION_RESTART,
    /**
	 * Go to the next playlist item.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NEXT,
    /**
	 * Go to the previous playlist item.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PREVIOUS,
}

[JsonConverter(typeof(ObsOutputStateConverter))]
public enum ObsOutputState {
    /**
	 * Unknown state.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_UNKNOWN,
    /**
	 * The output is starting.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_STARTING,
    /**
	 * The input has started.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_STARTED,
    /**
	 * The output is stopping.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_STOPPING,
    /**
	 * The output has stopped.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_STOPPED,
    /**
	 * The output has disconnected and is reconnecting.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_RECONNECTING,
    /**
	 * The output has reconnected successfully.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_RECONNECTED,
    /**
	 * The output is now paused.
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_PAUSED,
    /**
	 * The output has been resumed (unpaused).
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_WEBSOCKET_OUTPUT_RESUMED,
}

[JsonConverter(typeof(InputAudioMonitorTypeConverter))]
public enum InputAudioMonitorType
{
    /**
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_MONITORING_TYPE_NONE,
    /**
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_MONITORING_TYPE_MONITOR_ONLY,
    /**
	 * Initial OBS Version: 5.0.0
	 * Latest RPC Version: 1
	 */
    OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT,
}
