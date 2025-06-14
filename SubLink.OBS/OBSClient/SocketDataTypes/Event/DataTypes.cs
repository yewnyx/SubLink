using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Event;

public abstract class EventDataType {
    [JsonPropertyName("eventType")]
    public string EventType { get; set; } = string.Empty;
    /** The original intent required to be subscribed to in order to receive the event. */
    [JsonPropertyName("eventIntent")]
    public EventSubscription EventIntent { get; set; }
}

public class CurrentSceneCollectionChanging : EventDataType {
    public class EventDataType {
        /** Name of the current scene collection */
        [JsonPropertyName("sceneCollectionName")]
        public string SceneCollectionName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CurrentSceneCollectionChanged : EventDataType {
    public class EventDataType {
        /** Name of the new scene collection */
        [JsonPropertyName("sceneCollectionName")]
        public string SceneCollectionName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneCollectionListChanged : EventDataType {
    public class EventDataType {
        /** Updated list of scene collections */
        [JsonPropertyName("sceneCollections")]
        public string[] SceneCollections { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CurrentProfileChanging : EventDataType {
    public class EventDataType {
        /** Name of the current profile */
        [JsonPropertyName("profileName")]
        public string ProfileName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CurrentProfileChanged : EventDataType {
    public class EventDataType {
        /** Name of the new profile */
        [JsonPropertyName("profileName")]
        public string ProfileName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class ProfileListChanged : EventDataType {
    public class EventDataType {
        /** Updated list of profiles */
        [JsonPropertyName("profiles")]
        public string[] Profiles { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SourceFilterListReindexed : EventDataType {
    public class EventDataType {
        /** Name of the source */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** Array of filter objects */
        [JsonPropertyName("filters")]
        public JsonObject[] Filters { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SourceFilterCreated : EventDataType {
    public class EventDataType {
        /** Name of the source the filter was added to */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** Name of the filter */
        [JsonPropertyName("filterName")]
        public string FilterName { get; set; } = string.Empty;
        /** The kind of the filter */
        [JsonPropertyName("filterKind")]
        public string FilterKind { get; set; } = string.Empty;
        /** Index position of the filter */
        [JsonPropertyName("filterIndex")]
        public uint FilterIndex { get; set; }
        /** The settings configured to the filter when it was created */
        [JsonPropertyName("filterSettings")]
        public JsonObject FilterSettings { get; set; } = [];
        /** The default settings for the filter */
        [JsonPropertyName("defaultFilterSettings")]
        public JsonObject DefaultFilterSettings { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SourceFilterRemoved : EventDataType {
    public class EventDataType {
        /** Name of the source the filter is on */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** Name of the filter */
        [JsonPropertyName("filterName")]
        public string FilterName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SourceFilterNameChanged : EventDataType {
    public class EventDataType {
        /** Name of the source the filter is on */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** Old name of the filter */
        [JsonPropertyName("oldFilterName")]
        public string OldFilterName { get; set; } = string.Empty;
        /** New name of the filter */
        [JsonPropertyName("filterName")]
        public string FilterName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SourceFilterSettingsChanged : EventDataType {
    public class EventDataType {
        /** Name of the source the filter is on */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** Name of the filter */
        [JsonPropertyName("filterName")]
        public string FilterName { get; set; } = string.Empty;
        /** New settings object of the filter */
        [JsonPropertyName("filterSettings")]
        public JsonObject FilterSettings { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SourceFilterEnableStateChanged : EventDataType {
    public class EventDataType {
        /** Name of the source the filter is on */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** Name of the filter */
        [JsonPropertyName("filterName")]
        public string FilterName { get; set; } = string.Empty;
        /** Whether the filter is enabled */
        [JsonPropertyName("filterEnabled")]
        public bool FilterEnabled { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class ExitStarted : EventDataType { }

public class InputCreated : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** The kind of the input */
        [JsonPropertyName("inputKind")]
        public string InputKind { get; set; } = string.Empty;
        /** The unversioned kind of input (aka no `_v2` stuff) */
        [JsonPropertyName("unversionedInputKind")]
        public string UnversionedInputKind { get; set; } = string.Empty;
        /** The settings configured to the input when it was created */
        [JsonPropertyName("inputSettings")]
        public JsonObject InputSettings { get; set; } = [];
        /** The default settings for the input */
        [JsonPropertyName("defaultInputSettings")]
        public JsonObject DefaultInputSettings { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputRemoved : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputNameChanged : EventDataType {
    public class EventDataType {
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** Old name of the input */
        [JsonPropertyName("oldInputName")]
        public string OldInputName { get; set; } = string.Empty;
        /** New name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputSettingsChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** New settings object of the input */
        [JsonPropertyName("inputSettings")]
        public JsonObject InputSettings { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputActiveStateChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** Whether the input is active */
        [JsonPropertyName("videoActive")]
        public bool VideoActive { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputShowStateChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** Whether the input is showing */
        [JsonPropertyName("videoShowing")]
        public bool VideoShowing { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputMuteStateChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** Whether the input is muted */
        [JsonPropertyName("inputMuted")]
        public bool InputMuted { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputVolumeChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** New volume level multiplier */
        [JsonPropertyName("inputVolumeMul")]
        public float InputVolumeMul { get; set; }
        /** New volume level in dB */
        [JsonPropertyName("inputVolumeDb")]
        public float InputVolumeDb { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputAudioBalanceChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** New audio balance value of the input */
        [JsonPropertyName("inputAudioBalance")]
        public float InputAudioBalance { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputAudioSyncOffsetChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** New sync offset in milliseconds */
        [JsonPropertyName("inputAudioSyncOffset")]
        public int InputAudioSyncOffset { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputAudioTracksChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** Object of audio tracks along with their associated enable states */
        [JsonPropertyName("inputAudioTracks")]
        public JsonObject InputAudioTracks { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputAudioMonitorTypeChanged : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** New monitor type of the input */
        [JsonPropertyName("monitorType")]
        public string MonitorType { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class InputVolumeMeters : EventDataType {
    public class EventDataType {
        /** Array of active inputs with their associated volume levels */
        [JsonPropertyName("inputs")]
        public JsonObject[] Inputs { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class MediaInputPlaybackStarted : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class MediaInputPlaybackEnded : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class MediaInputActionTriggered : EventDataType {
    public class EventDataType {
        /** Name of the input */
        [JsonPropertyName("inputName")]
        public string InputName { get; set; } = string.Empty;
        /** UUID of the input */
        [JsonPropertyName("inputUuid")]
        public string InputUuid { get; set; } = string.Empty;
        /** Action performed on the input. See `ObsMediaInputAction` enum */
        [JsonPropertyName("mediaAction")]
        public string MediaAction { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class StreamStateChanged : EventDataType {
    public class EventDataType {
        /** Whether the output is active */
        [JsonPropertyName("outputActive")]
        public bool OutputActive { get; set; }
        /** The specific state of the output */
        [JsonPropertyName("outputState")]
        public string OutputState { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class RecordStateChanged : EventDataType {
    public class EventDataType {
        /** Whether the output is active */
        [JsonPropertyName("outputActive")]
        public bool OutputActive { get; set; }
        /** The specific state of the output */
        [JsonPropertyName("outputState")]
        public string OutputState { get; set; } = string.Empty;
        /** File name for the saved recording, if record stopped. `null` otherwise */
        [JsonPropertyName("outputPath")]
        public string? OutputPath { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class RecordFileChanged : EventDataType {
    public class EventDataType {
        /** File name that the output has begun writing to */
        [JsonPropertyName("newOutputPath")]
        public string NewOutputPath { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class ReplayBufferStateChanged : EventDataType {
    public class EventDataType {
        /** Whether the output is active */
        [JsonPropertyName("outputActive")]
        public bool OutputActive { get; set; }
        /** The specific state of the output */
        [JsonPropertyName("outputState")]
        public string OutputState { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class VirtualcamStateChanged : EventDataType {
    public class EventDataType {
        /** Whether the output is active */
        [JsonPropertyName("outputActive")]
        public bool OutputActive { get; set; }
        /** The specific state of the output */
        [JsonPropertyName("outputState")]
        public string OutputState { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class ReplayBufferSaved : EventDataType {
    public class EventDataType {
        /** Path of the saved replay file */
        [JsonPropertyName("savedReplayPath")]
        public string SavedReplayPath { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneItemCreated : EventDataType {
    public class EventDataType {
        /** Name of the scene the item was added to */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene the item was added to */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Name of the underlying source (input/scene) */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** UUID of the underlying source (input/scene) */
        [JsonPropertyName("sourceUuid")]
        public string SourceUuid { get; set; } = string.Empty;
        /** Numeric ID of the scene item */
        [JsonPropertyName("sceneItemId")]
        public uint SceneItemId { get; set; }
        /** Index position of the item */
        [JsonPropertyName("sceneItemIndex")]
        public uint SceneItemIndex { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneItemRemoved : EventDataType {
    public class EventDataType {
        /** Name of the scene the item was removed from */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene the item was removed from */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Name of the underlying source (input/scene) */
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; } = string.Empty;
        /** UUID of the underlying source (input/scene) */
        [JsonPropertyName("sourceUuid")]
        public string SourceUuid { get; set; } = string.Empty;
        /** Numeric ID of the scene item */
        [JsonPropertyName("sceneItemId")]
        public uint SceneItemId { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneItemListReindexed : EventDataType {
    public class EventDataType {
        /** Name of the scene */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Array of scene item objects */
        [JsonPropertyName("sceneItems")]
        public JsonObject[] SceneItems { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneItemEnableStateChanged : EventDataType {
    public class EventDataType {
        /** Name of the scene the item is in */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene the item is in */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Numeric ID of the scene item */
        [JsonPropertyName("sceneItemId")]
        public uint SceneItemId { get; set; }
        /** Whether the scene item is enabled (visible) */
        [JsonPropertyName("sceneItemEnabled")]
        public bool SceneItemEnabled { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneItemLockStateChanged : EventDataType {
    public class EventDataType {
        /** Name of the scene the item is in */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene the item is in */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Numeric ID of the scene item */
        [JsonPropertyName("sceneItemId")]
        public uint SceneItemId { get; set; }
        /** Whether the scene item is locked */
        [JsonPropertyName("sceneItemLocked")]
        public bool SceneItemLocked { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneItemSelected : EventDataType {
    public class EventDataType {
        /** Name of the scene the item is in */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene the item is in */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Numeric ID of the scene item */
        [JsonPropertyName("sceneItemId")]
        public uint SceneItemId { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneItemTransformChanged : EventDataType {
    public class EventDataType {
        /** The name of the scene the item is in */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** The UUID of the scene the item is in */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Numeric ID of the scene item */
        [JsonPropertyName("sceneItemId")]
        public uint SceneItemId { get; set; }
        /** New transform/crop info of the scene item */
        [JsonPropertyName("sceneItemTransform")]
        public JsonObject SceneItemTransform { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneCreated : EventDataType {
    public class EventDataType {
        /** Name of the new scene */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the new scene */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Whether the new scene is a group */
        [JsonPropertyName("isGroup")]
        public bool IsGroup { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneRemoved : EventDataType {
    public class EventDataType {
        /** Name of the removed scene */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the removed scene */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Whether the scene was a group */
        [JsonPropertyName("isGroup")]
        public bool IsGroup { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneNameChanged : EventDataType {
    public class EventDataType {
        /** UUID of the scene */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
        /** Old name of the scene */
        [JsonPropertyName("oldSceneName")]
        public string OldSceneName { get; set; } = string.Empty;
        /** New name of the scene */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CurrentProgramSceneChanged : EventDataType {
    public class EventDataType {
        /** Name of the scene that was switched to */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene that was switched to */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CurrentPreviewSceneChanged : EventDataType {
    public class EventDataType {
        /** Name of the scene that was switched to */
        [JsonPropertyName("sceneName")]
        public string SceneName { get; set; } = string.Empty;
        /** UUID of the scene that was switched to */
        [JsonPropertyName("sceneUuid")]
        public string SceneUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneListChanged : EventDataType {
    public class EventDataType {
        /** Updated array of scenes */
        [JsonPropertyName("scenes")]
        public JsonObject[] Scenes { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CurrentSceneTransitionChanged : EventDataType {
    public class EventDataType {
        /** Name of the new transition */
        [JsonPropertyName("transitionName")]
        public string TransitionName { get; set; } = string.Empty;
        /** UUID of the new transition */
        [JsonPropertyName("transitionUuid")]
        public string TransitionUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CurrentSceneTransitionDurationChanged : EventDataType {
    public class EventDataType {
        /** Transition duration in milliseconds */
        [JsonPropertyName("transitionDuration")]
        public uint TransitionDuration { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneTransitionStarted : EventDataType {
    public class EventDataType {
        /** Scene transition name */
        [JsonPropertyName("transitionName")]
        public string TransitionName { get; set; } = string.Empty;
        /** Scene transition UUID */
        [JsonPropertyName("transitionUuid")]
        public string TransitionUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneTransitionEnded : EventDataType {
    public class EventDataType {
        /** Scene transition name */
        [JsonPropertyName("transitionName")]
        public string TransitionName { get; set; } = string.Empty;
        /** Scene transition UUID */
        [JsonPropertyName("transitionUuid")]
        public string TransitionUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class SceneTransitionVideoEnded : EventDataType {
    public class EventDataType {
        /** Scene transition name */
        [JsonPropertyName("transitionName")]
        public string TransitionName { get; set; } = string.Empty;
        /** Scene transition UUID */
        [JsonPropertyName("transitionUuid")]
        public string TransitionUuid { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class StudioModeStateChanged : EventDataType {
    public class EventDataType {
        /** True == Enabled, False == Disabled */
        [JsonPropertyName("studioModeEnabled")]
        public bool StudioModeEnabled { get; set; }
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class ScreenshotSaved : EventDataType {
    public class EventDataType {
        /** Path of the saved image file */
        [JsonPropertyName("savedScreenshotPath")]
        public string SavedScreenshotPath { get; set; } = string.Empty;
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class VendorEvent : EventDataType {
    public class EventDataType {
        /** Name of the vendor emitting the event */
        [JsonPropertyName("vendorName")]
        public string VendorName { get; set; } = string.Empty;
        /** Vendor-provided event typedef */
        [JsonPropertyName("eventType")]
        public string EventType { get; set; } = string.Empty;
        /** Vendor-provided event data. {} if event does not provide any data */
        [JsonPropertyName("eventData")]
        public JsonObject EventData { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}

public class CustomEvent : EventDataType {
    public class EventDataType {
        /** Custom event data */
        [JsonPropertyName("eventData")]
        public JsonObject EventData { get; set; } = [];
    }

    [JsonPropertyName("eventData")]
    public EventDataType EventData { get; set; } = new();
}
