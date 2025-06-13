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
    public float InputVolumeDb { get; set; }
}

public class GetInputAudioSyncOffset : IResponseType {
    /** Audio sync offset in milliseconds */
    [JsonPropertyName("inputAudioSyncOffset")]
    public uint InputAudioSyncOffset { get; set; }
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
