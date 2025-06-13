using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
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
    public abstract class BaseEventType {
        [JsonPropertyName("eventType")]
        public string EventType { get; set; } = string.Empty;
        /**
        * The original intent required to be subscribed to in order to receive the event.
        */
        [JsonPropertyName("eventIntent")]
        public EventSubscription EventIntent { get; set; }
    }

    public class CurrentSceneCollectionChanging : BaseEventType {
        public class EventDataType {
            /**
            * Name of the current scene collection
            */
            [JsonPropertyName("sceneCollectionName")]
            public string SceneCollectionName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CurrentSceneCollectionChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the new scene collection
            */
            [JsonPropertyName("sceneCollectionName")]
            public string SceneCollectionName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneCollectionListChanged : BaseEventType {
        public class EventDataType {
            /**
            * Updated list of scene collections
            */
            [JsonPropertyName("sceneCollections")]
            public string[] SceneCollections { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CurrentProfileChanging : BaseEventType {
        public class EventDataType {
            /**
            * Name of the current profile
            */
            [JsonPropertyName("profileName")]
            public string ProfileName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CurrentProfileChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the new profile
            */
            [JsonPropertyName("profileName")]
            public string ProfileName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class ProfileListChanged : BaseEventType {
        public class EventDataType {
            /**
            * Updated list of profiles
            */
            [JsonPropertyName("profiles")]
            public string[] Profiles { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SourceFilterListReindexed : BaseEventType {
        public class EventDataType {
            /**
            * Name of the source
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * Array of filter objects
            */
            [JsonPropertyName("filters")]
            public JsonObject[] Filters { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SourceFilterCreated : BaseEventType {
        public class EventDataType {
            /**
            * Name of the source the filter was added to
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * Name of the filter
            */
            [JsonPropertyName("filterName")]
            public string FilterName { get; set; } = string.Empty;
            /**
            * The kind of the filter
            */
            [JsonPropertyName("filterKind")]
            public string FilterKind { get; set; } = string.Empty;
            /**
            * Index position of the filter
            */
            [JsonPropertyName("filterIndex")]
            public uint FilterIndex { get; set; }
            /**
            * The settings configured to the filter when it was created
            */
            [JsonPropertyName("filterSettings")]
            public JsonObject FilterSettings { get; set; } = [];
            /**
            * The default settings for the filter
            */
            [JsonPropertyName("defaultFilterSettings")]
            public JsonObject DefaultFilterSettings { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SourceFilterRemoved : BaseEventType {
        public class EventDataType {
            /**
            * Name of the source the filter is on
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * Name of the filter
            */
            [JsonPropertyName("filterName")]
            public string FilterName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SourceFilterNameChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the source the filter is on
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * Old name of the filter
            */
            [JsonPropertyName("oldFilterName")]
            public string OldFilterName { get; set; } = string.Empty;
            /**
            * New name of the filter
            */
            [JsonPropertyName("filterName")]
            public string FilterName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SourceFilterSettingsChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the source the filter is on
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * Name of the filter
            */
            [JsonPropertyName("filterName")]
            public string FilterName { get; set; } = string.Empty;
            /**
            * New settings object of the filter
            */
            [JsonPropertyName("filterSettings")]
            public JsonObject FilterSettings { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

    public class SourceFilterEnableStateChanged : BaseEventType
    {
        public class EventDataType
        {
            /**
            * Name of the source the filter is on
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * Name of the filter
            */
            [JsonPropertyName("filterName")]
            public string FilterName { get; set; } = string.Empty;
            /**
            * Whether the filter is enabled
            */
            [JsonPropertyName("filterEnabled")]
            public bool FilterEnabled { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
    }

	public class ExitStarted : BaseEventType { }

	public class InputCreated : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * The kind of the input
            */
            [JsonPropertyName("inputKind")]
            public string InputKind { get; set; } = string.Empty;
            /**
            * The unversioned kind of input (aka no `_v2` stuff)
            */
            [JsonPropertyName("unversionedInputKind")]
            public string UnversionedInputKind { get; set; } = string.Empty;
            /**
            * The settings configured to the input when it was created
            */
            [JsonPropertyName("inputSettings")]
            public JsonObject InputSettings { get; set; } = [];
            /**
            * The default settings for the input
            */
            [JsonPropertyName("defaultInputSettings")]
            public JsonObject DefaultInputSettings { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputRemoved : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputNameChanged : BaseEventType {
        public class EventDataType {
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * Old name of the input
            */
            [JsonPropertyName("oldInputName")]
            public string OldInputName { get; set; } = string.Empty;
            /**
            * New name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputSettingsChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * New settings object of the input
            */
            [JsonPropertyName("inputSettings")]
            public JsonObject InputSettings { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputActiveStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * Whether the input is active
            */
            [JsonPropertyName("videoActive")]
            public bool VideoActive { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputShowStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * Whether the input is showing
            */
            [JsonPropertyName("videoShowing")]
            public bool VideoShowing { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputMuteStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * Whether the input is muted
            */
            [JsonPropertyName("inputMuted")]
            public bool InputMuted { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputVolumeChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * New volume level multiplier
            */
            [JsonPropertyName("inputVolumeMul")]
            public float InputVolumeMul { get; set; }
            /**
            * New volume level in dB
            */
            [JsonPropertyName("inputVolumeDb")]
            public float InputVolumeDb { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputAudioBalanceChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * New audio balance value of the input
            */
            [JsonPropertyName("inputAudioBalance")]
            public float InputAudioBalance { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputAudioSyncOffsetChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * New sync offset in milliseconds
            */
            [JsonPropertyName("inputAudioSyncOffset")]
            public int InputAudioSyncOffset { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputAudioTracksChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * Object of audio tracks along with their associated enable states
            */
            [JsonPropertyName("inputAudioTracks")]
            public JsonObject InputAudioTracks { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputAudioMonitorTypeChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * New monitor type of the input
            */
            [JsonPropertyName("monitorType")]
            public string MonitorType { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class InputVolumeMeters : BaseEventType {
        public class EventDataType {
            /**
            * Array of active inputs with their associated volume levels
            */
            [JsonPropertyName("inputs")]
            public JsonObject[] Inputs { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class MediaInputPlaybackStarted : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class MediaInputPlaybackEnded : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class MediaInputActionTriggered : BaseEventType {
        public class EventDataType {
            /**
            * Name of the input
            */
            [JsonPropertyName("inputName")]
            public string InputName { get; set; } = string.Empty;
            /**
            * UUID of the input
            */
            [JsonPropertyName("inputUuid")]
            public string InputUuid { get; set; } = string.Empty;
            /**
            * Action performed on the input. See `ObsMediaInputAction` enum
            */
            [JsonPropertyName("mediaAction")]
            public string MediaAction { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class StreamStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Whether the output is active
            */
            [JsonPropertyName("outputActive")]
            public bool OutputActive { get; set; }
            /**
            * The specific state of the output
            */
            [JsonPropertyName("outputState")]
            public string OutputState { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class RecordStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Whether the output is active
            */
            [JsonPropertyName("outputActive")]
            public bool OutputActive { get; set; }
            /**
            * The specific state of the output
            */
            [JsonPropertyName("outputState")]
            public string OutputState { get; set; } = string.Empty;
            /**
            * File name for the saved recording, if record stopped. `null` otherwise
            */
            [JsonPropertyName("outputPath")]
            public string? OutputPath { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class RecordFileChanged : BaseEventType {
        public class EventDataType {
            /**
            * File name that the output has begun writing to
            */
            [JsonPropertyName("newOutputPath")]
            public string NewOutputPath { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class ReplayBufferStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Whether the output is active
            */
            [JsonPropertyName("outputActive")]
            public bool OutputActive { get; set; }
            /**
            * The specific state of the output
            */
            [JsonPropertyName("outputState")]
            public string OutputState { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class VirtualcamStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Whether the output is active
            */
            [JsonPropertyName("outputActive")]
            public bool OutputActive { get; set; }
            /**
            * The specific state of the output
            */
            [JsonPropertyName("outputState")]
            public string OutputState { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class ReplayBufferSaved : BaseEventType {
        public class EventDataType {
            /**
            * Path of the saved replay file
            */
            [JsonPropertyName("savedReplayPath")]
            public string SavedReplayPath { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneItemCreated : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene the item was added to
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene the item was added to
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Name of the underlying source (input/scene)
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * UUID of the underlying source (input/scene)
            */
            [JsonPropertyName("sourceUuid")]
            public string SourceUuid { get; set; } = string.Empty;
            /**
            * Numeric ID of the scene item
            */
            [JsonPropertyName("sceneItemId")]
            public uint SceneItemId { get; set; }
            /**
            * Index position of the item
            */
            [JsonPropertyName("sceneItemIndex")]
            public uint SceneItemIndex { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneItemRemoved : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene the item was removed from
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene the item was removed from
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Name of the underlying source (input/scene)
            */
            [JsonPropertyName("sourceName")]
            public string SourceName { get; set; } = string.Empty;
            /**
            * UUID of the underlying source (input/scene)
            */
            [JsonPropertyName("sourceUuid")]
            public string SourceUuid { get; set; } = string.Empty;
            /**
            * Numeric ID of the scene item
            */
            [JsonPropertyName("sceneItemId")]
            public uint SceneItemId { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneItemListReindexed : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Array of scene item objects
            */
            [JsonPropertyName("sceneItems")]
            public JsonObject[] SceneItems { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneItemEnableStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene the item is in
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene the item is in
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Numeric ID of the scene item
            */
            [JsonPropertyName("sceneItemId")]
            public uint SceneItemId { get; set; }
            /**
            * Whether the scene item is enabled (visible)
            */
            [JsonPropertyName("sceneItemEnabled")]
            public bool SceneItemEnabled { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneItemLockStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene the item is in
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene the item is in
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Numeric ID of the scene item
            */
            [JsonPropertyName("sceneItemId")]
            public uint SceneItemId { get; set; }
            /**
            * Whether the scene item is locked
            */
            [JsonPropertyName("sceneItemLocked")]
            public bool SceneItemLocked { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneItemSelected : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene the item is in
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene the item is in
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Numeric ID of the scene item
            */
            [JsonPropertyName("sceneItemId")]
            public uint SceneItemId { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneItemTransformChanged : BaseEventType {
        public class EventDataType {
            /**
            * The name of the scene the item is in
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * The UUID of the scene the item is in
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Numeric ID of the scene item
            */
            [JsonPropertyName("sceneItemId")]
            public uint SceneItemId { get; set; }
            /**
            * New transform/crop info of the scene item
            */
            [JsonPropertyName("sceneItemTransform")]
            public JsonObject SceneItemTransform { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneCreated : BaseEventType {
        public class EventDataType {
            /**
            * Name of the new scene
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the new scene
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Whether the new scene is a group
            */
            [JsonPropertyName("isGroup")]
            public bool IsGroup { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneRemoved : BaseEventType {
        public class EventDataType {
            /**
            * Name of the removed scene
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the removed scene
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Whether the scene was a group
            */
            [JsonPropertyName("isGroup")]
            public bool IsGroup { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneNameChanged : BaseEventType {
        public class EventDataType {
            /**
            * UUID of the scene
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
            /**
            * Old name of the scene
            */
            [JsonPropertyName("oldSceneName")]
            public string OldSceneName { get; set; } = string.Empty;
            /**
            * New name of the scene
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CurrentProgramSceneChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene that was switched to
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene that was switched to
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CurrentPreviewSceneChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the scene that was switched to
            */
            [JsonPropertyName("sceneName")]
            public string SceneName { get; set; } = string.Empty;
            /**
            * UUID of the scene that was switched to
            */
            [JsonPropertyName("sceneUuid")]
            public string SceneUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneListChanged : BaseEventType {
        public class EventDataType {
            /**
            * Updated array of scenes
            */
            [JsonPropertyName("scenes")]
            public JsonObject[] Scenes { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CurrentSceneTransitionChanged : BaseEventType {
        public class EventDataType {
            /**
            * Name of the new transition
            */
            [JsonPropertyName("transitionName")]
            public string TransitionName { get; set; } = string.Empty;
            /**
            * UUID of the new transition
            */
            [JsonPropertyName("transitionUuid")]
            public string TransitionUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CurrentSceneTransitionDurationChanged : BaseEventType {
        public class EventDataType {
            /**
            * Transition duration in milliseconds
            */
            [JsonPropertyName("transitionDuration")]
            public uint TransitionDuration { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneTransitionStarted : BaseEventType {
        public class EventDataType {
            /**
            * Scene transition name
            */
            [JsonPropertyName("transitionName")]
            public string TransitionName { get; set; } = string.Empty;
            /**
            * Scene transition UUID
            */
            [JsonPropertyName("transitionUuid")]
            public string TransitionUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneTransitionEnded : BaseEventType {
        public class EventDataType {
            /**
            * Scene transition name
            */
            [JsonPropertyName("transitionName")]
            public string TransitionName { get; set; } = string.Empty;
            /**
            * Scene transition UUID
            */
            [JsonPropertyName("transitionUuid")]
            public string TransitionUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class SceneTransitionVideoEnded : BaseEventType {
        public class EventDataType {
            /**
            * Scene transition name
            */
            [JsonPropertyName("transitionName")]
            public string TransitionName { get; set; } = string.Empty;
            /**
            * Scene transition UUID
            */
            [JsonPropertyName("transitionUuid")]
            public string TransitionUuid { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class StudioModeStateChanged : BaseEventType {
        public class EventDataType {
            /**
            * True == Enabled, False == Disabled
            */
            [JsonPropertyName("studioModeEnabled")]
            public bool StudioModeEnabled { get; set; }
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class ScreenshotSaved : BaseEventType {
        public class EventDataType {
            /**
            * Path of the saved image file
            */
            [JsonPropertyName("savedScreenshotPath")]
            public string SavedScreenshotPath { get; set; } = string.Empty;
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class VendorEvent : BaseEventType {
        public class EventDataType {
            /**
            * Name of the vendor emitting the event
            */
            [JsonPropertyName("vendorName")]
            public string VendorName { get; set; } = string.Empty;
            /**
            * Vendor-provided event typedef
            */
            [JsonPropertyName("eventType")]
            public string EventType { get; set; } = string.Empty;
            /**
            * Vendor-provided event data. {} if event does not provide any data
            */
            [JsonPropertyName("eventData")]
            public JsonObject EventData { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

	public class CustomEvent : BaseEventType {
        public class EventDataType {
            /**
            * Custom event data
            */
            [JsonPropertyName("eventData")]
            public JsonObject EventData { get; set; } = [];
        }

        [JsonPropertyName("eventData")]
        public EventDataType EventData { get; set; } = new();
	}

    [JsonPropertyName("d")]
    [JsonConverter(typeof(EventTypeConverter))]
    public BaseEventType? D { get; set; }
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
