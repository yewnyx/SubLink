using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Response;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
[JsonDerivedType(typeof(GetInputMute))]
[JsonDerivedType(typeof(ToggleInputMute))]
[JsonDerivedType(typeof(GetInputVolume))]
[JsonDerivedType(typeof(GetInputAudioSyncOffset))]
[JsonDerivedType(typeof(GetSceneItemEnabled))]
[JsonDerivedType(typeof(GetCurrentProgramScene))]
[JsonDerivedType(typeof(GetCurrentPreviewScene))]
[JsonDerivedType(typeof(GetCurrentSceneTransition))]
[JsonDerivedType(typeof(GetStudioModeEnabled))]
public interface IResponseType { }

public class GetInputMute : IResponseType {
    /** Whether the input is muted */
    [JsonPropertyName("inputMuted")]
    public bool InputMuted { get; set; }
}

public class ToggleInputMute : IResponseType {
    /** Whether the input has been muted or unmuted */
    [JsonPropertyName("inputMuted")]
    public bool InputMuted { get; set; }
}

public class GetInputVolume : IResponseType {
    /** Volume setting in mul */
    [JsonPropertyName("inputVolumeMul")]
    public float InputVolumeMul { get; set; }
    /** Volume setting in dB */
    [JsonPropertyName("inputVolumeDb")]
    public float InputVolumeDB { get; set; }
}

public class GetInputAudioSyncOffset : IResponseType {
    /** Audio sync offset in milliseconds */
    [JsonPropertyName("inputAudioSyncOffset")]
    public int InputAudioSyncOffset { get; set; }
}

public class GetSceneItemEnabled : IResponseType {
    /** Whether the scene item is enabled. `true` for enabled, `false` for disabled */
    [JsonPropertyName("sceneItemEnabled")]
    public bool SceneItemEnabled { get; set; }
}

public class GetCurrentProgramScene : IResponseType {
    /** Current program scene name */
    [JsonPropertyName("sceneName")]
    public string SceneName { get; set; } = string.Empty;
    /** Current program scene UUID */
    [JsonPropertyName("sceneUuid")]
    public string SceneUuid { get; set; } = string.Empty;
    /** Current program scene name (Deprecated) */
    [JsonPropertyName("currentProgramSceneName")]
    public string? CurrentProgramSceneName { get; set; }
    /** Current program scene UUID (Deprecated) */
    [JsonPropertyName("currentProgramSceneUuid")]
    public string? CurrentProgramSceneUuid { get; set; }
}

public class GetCurrentPreviewScene : IResponseType {
    /** Current preview scene name */
    [JsonPropertyName("sceneName")]
    public string SceneName { get; set; } = string.Empty;
    /** Current preview scene UUID */
    [JsonPropertyName("sceneUuid")]
    public string SceneUuid { get; set; } = string.Empty;
    /** Current preview scene name (Deprecated) */
    [JsonPropertyName("currentPreviewSceneName")]
    public string? CurrentPreviewSceneName { get; set; }
    /** Current preview scene UUID (Deprecated) */
    [JsonPropertyName("currentPreviewSceneUuid")]
    public string? CurrentPreviewSceneUuid { get; set; }
}

public class GetCurrentSceneTransition : IResponseType {
    /** Name of the transition */
    [JsonPropertyName("transitionName")]
    public string TransitionName { get; set; } = string.Empty;
    /** UUID of the transition */
    [JsonPropertyName("transitionUuid")]
    public string TransitionUuid { get; set; } = string.Empty;
    /** Kind of the transition */
    [JsonPropertyName("transitionKind")]
    public string TransitionKind { get; set; } = string.Empty;
    /** Whether the transition uses a fixed (unconfigurable) duration */
    [JsonPropertyName("transitionFixed")]
    public bool TransitionFixed { get; set; }
    /** Configured transition duration in milliseconds. `null` if transition is fixed */
    [JsonPropertyName("transitionDuration")]
    public uint? TransitionDuration { get; set; }
    /** Whether the transition supports being configured */
    [JsonPropertyName("transitionConfigurable")]
    public bool TransitionConfigurable { get; set; }
    /** Object of settings for the transition. `null` if transition is not configurable */
    [JsonPropertyName("transitionSettings")]
    public JsonObject TransitionSettings { get; set; } = [];
}

public class GetStudioModeEnabled : IResponseType {
    /** Whether studio mode is enabled */
    [JsonPropertyName("studioModeEnabled")]
    public bool StudioModeEnabled { get; set; }
}
