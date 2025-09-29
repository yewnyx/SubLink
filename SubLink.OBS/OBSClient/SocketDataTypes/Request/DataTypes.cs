using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Request;

// https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#requests
[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
[JsonDerivedType(typeof(SetSourceFilterEnabled))]
[JsonDerivedType(typeof(GetHotkeyList))]
[JsonDerivedType(typeof(TriggerHotkeyByName))]
[JsonDerivedType(typeof(TriggerHotkeyByKeySequence))]
[JsonDerivedType(typeof(GetInputMute))]
[JsonDerivedType(typeof(SetInputMute))]
[JsonDerivedType(typeof(ToggleInputMute))]
[JsonDerivedType(typeof(GetInputVolume))]
[JsonDerivedType(typeof(SetInputVolume))]
[JsonDerivedType(typeof(GetInputAudioSyncOffset))]
[JsonDerivedType(typeof(SetInputAudioSyncOffset))]
[JsonDerivedType(typeof(GetSceneItemEnabled))]
[JsonDerivedType(typeof(SetSceneItemEnabled))]
[JsonDerivedType(typeof(GetCurrentProgramScene))]
[JsonDerivedType(typeof(SetCurrentProgramScene))]
[JsonDerivedType(typeof(GetCurrentPreviewScene))]
[JsonDerivedType(typeof(SetCurrentPreviewScene))]
[JsonDerivedType(typeof(GetCurrentSceneTransition))]
[JsonDerivedType(typeof(SetCurrentSceneTransition))]
[JsonDerivedType(typeof(TriggerStudioModeTransition))]
[JsonDerivedType(typeof(GetStudioModeEnabled))]
public interface IRequestDataType { }

public class SetSourceFilterEnabled : IRequestDataType
{
    /** Name of the source */
    [JsonPropertyName("sourceName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceName { get; set; }
    /** UUID of the source */
    [JsonPropertyName("sourceUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceUuid { get; set; }
    /** Name of the filter */
    [JsonPropertyName("filterName")]
    public string FilterName { get; set; } = string.Empty;
    /** New enable state of the filter */
    [JsonPropertyName("filterEnabled")]
    public bool FilterEnabled { get; set; }
}

public class GetHotkeyList : IRequestDataType { }

public class TriggerHotkeyByName : IRequestDataType {
    /** Name of the hotkey to trigger */
    [JsonPropertyName("hotkeyName")]
    public string HotkeyName { get; set; } = string.Empty;
    /** Name of context of the hotkey to trigger */
    [JsonPropertyName("contextName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContextName { get; set; }
}

public class TriggerHotkeyByKeySequence : IRequestDataType {
    public class KeyModifiersObj {
        /** Press Shift */
        [JsonPropertyName("shift")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Shift { get; set; }
        /** Press CTRL */
        [JsonPropertyName("control")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Control { get; set; }
        /** Press ALT */
        [JsonPropertyName("alt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Alt { get; set; }
        /** Press CMD (Mac) */
        [JsonPropertyName("command")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Command { get; set; }
    }

    /** The OBS key ID to use. See https://github.com/obsproject/obs-studio/blob/master/libobs/obs-hotkeys.h */
    [JsonPropertyName("keyId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? KeyId { get; set; }
    /** Object containing key modifiers to apply */
    [JsonPropertyName("keyModifiers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public KeyModifiersObj? KeyModifiers { get; set; }
}

public class GetInputMute : IRequestDataType {
    /** Name of input to get the mute state of */
    [JsonPropertyName("inputName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputName { get; set; }
    /** UUID of input to get the mute state of */
    [JsonPropertyName("inputUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputUuid { get; set; }
}

public class SetInputMute : IRequestDataType {
    /** Name of the input to set the mute state of */
    [JsonPropertyName("inputName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputName { get; set; }
    /** UUID of the input to set the mute state of */
    [JsonPropertyName("inputUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputUuid { get; set; }
    /** Whether to mute the input or not */
    [JsonPropertyName("inputMuted")]
    public bool InputMuted { get; set; }
}

public class ToggleInputMute : IRequestDataType {
    /** Name of the input to toggle the mute state of */
    [JsonPropertyName("inputName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputName { get; set; }
    /** UUID of the input to toggle the mute state of */
    [JsonPropertyName("inputUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputUuid { get; set; }
}

public class GetInputVolume : IRequestDataType {
    /** Name of the input to get the volume of */
    [JsonPropertyName("inputName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputName { get; set; }
    /** UUID of the input to get the volume of */
    [JsonPropertyName("inputUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputUuid { get; set; }
}

public class SetInputVolume : IRequestDataType {
    /** Name of the input to set the volume of */
    [JsonPropertyName("inputName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputName { get; set; }
    /** UUID of the input to set the volume of */
    [JsonPropertyName("inputUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputUuid { get; set; }
    /** Volume setting in mul @restrictions >= 0, <= 20 */
    [JsonPropertyName("inputVolumeMul")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? InputVolumeMul { get; set; }
    /** Volume setting in dB @restrictions >= -100, <= 26 */
    [JsonPropertyName("inputVolumeDb")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? InputVolumeDB { get; set; }
}

public class GetInputAudioSyncOffset : IRequestDataType {
    /** Name of the input to get the audio sync offset of */
    [JsonPropertyName("inputName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputName { get; set; }
    /** UUID of the input to get the audio sync offset of */
    [JsonPropertyName("inputUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputUuid { get; set; }
}

public class SetInputAudioSyncOffset : IRequestDataType {
    /** Name of the input to set the audio sync offset of */
    [JsonPropertyName("inputName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputName { get; set; }
    /** UUID of the input to set the audio sync offset of */
    [JsonPropertyName("inputUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputUuid { get; set; }
    /** New audio sync offset in milliseconds @restrictions >= -950, <= 20000 */
    [JsonPropertyName("inputAudioSyncOffset")]
    public int InputAudioSyncOffset { get; set; } = 0;
}

public class GetSceneItemEnabled : IRequestDataType {
    /** Name of the scene the item is in */
    [JsonPropertyName("sceneName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneName { get; set; }
    /** UUID of the scene the item is in */
    [JsonPropertyName("sceneUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneUuid { get; set; }
    /** Numeric ID of the scene item @restrictions >= 0 */
    [JsonPropertyName("sceneItemId")]
    public uint SceneItemId { get; set; } = 0;
}

public class SetSceneItemEnabled : IRequestDataType {
    /** Name of the scene the item is in */
    [JsonPropertyName("sceneName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneName { get; set; }
    /** UUID of the scene the item is in */
    [JsonPropertyName("sceneUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneUuid { get; set; }
    /** Numeric ID of the scene item @restrictions >= 0 */
    [JsonPropertyName("sceneItemId")]
    public uint SceneItemId { get; set; } = 0;
    /** New enable state of the scene item */
    [JsonPropertyName("sceneItemEnabled")]
    public bool SceneItemEnabled { get; set; } = false;
}

public class GetCurrentProgramScene : IRequestDataType { }

public class SetCurrentProgramScene : IRequestDataType {
    /** Scene name to set as the current program scene */
    [JsonPropertyName("sceneName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneName { get; set; }
    /** Scene UUID to set as the current program scene */
    [JsonPropertyName("sceneUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneUuid { get; set; }
}

public class GetCurrentPreviewScene : IRequestDataType { }

public class SetCurrentPreviewScene : IRequestDataType {
    /** Scene name to set as the current preview scene */
    [JsonPropertyName("sceneName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneName { get; set; }
    /** Scene UUID to set as the current preview scene */
    [JsonPropertyName("sceneUuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SceneUuid { get; set; }
}

public class GetCurrentSceneTransition : IRequestDataType { }

public class SetCurrentSceneTransition : IRequestDataType {
    /** Name of the transition to make active */
    [JsonPropertyName("transitionName")]
    public string TransitionName { get; set; } = string.Empty;
}

public class TriggerStudioModeTransition : IRequestDataType { }

public class GetStudioModeEnabled : IRequestDataType { }