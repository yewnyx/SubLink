using System;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Event;

internal sealed class OBSErrorArgs : EventArgs {
    public Exception Exception { get; set; } = new();

    public OBSErrorArgs() { }

    public OBSErrorArgs(Exception exception) {
        Exception = exception;
    }
}

public class OBSEventArgs<T> : EventArgs where T : new() {
    public T Data { get; set; } = new T();
}

internal sealed class CurrentSceneCollectionChangingArgs : OBSEventArgs<CurrentSceneCollectionChanging.EventDataType> { }
internal sealed class CurrentSceneCollectionChangedArgs : OBSEventArgs<CurrentSceneCollectionChanged.EventDataType> { }
internal sealed class SceneCollectionListChangedArgs : OBSEventArgs<SceneCollectionListChanged.EventDataType> { }
internal sealed class CurrentProfileChangingArgs : OBSEventArgs<CurrentProfileChanging.EventDataType> { }
internal sealed class CurrentProfileChangedArgs : OBSEventArgs<CurrentProfileChanged.EventDataType> { }
internal sealed class ProfileListChangedArgs : OBSEventArgs<ProfileListChanged.EventDataType> { }
internal sealed class SourceFilterListReindexedArgs : OBSEventArgs<SourceFilterListReindexed.EventDataType> { }
internal sealed class SourceFilterCreatedArgs : OBSEventArgs<SourceFilterCreated.EventDataType> { }
internal sealed class SourceFilterRemovedArgs : OBSEventArgs<SourceFilterRemoved.EventDataType> { }
internal sealed class SourceFilterNameChangedArgs : OBSEventArgs<SourceFilterNameChanged.EventDataType> { }
internal sealed class SourceFilterSettingsChangedArgs : OBSEventArgs<SourceFilterSettingsChanged.EventDataType> { }
internal sealed class SourceFilterEnableStateChangedArgs : OBSEventArgs<SourceFilterEnableStateChanged.EventDataType> { }
internal sealed class InputCreatedArgs : OBSEventArgs<InputCreated.EventDataType> { }
internal sealed class InputRemovedArgs : OBSEventArgs<InputRemoved.EventDataType> { }
internal sealed class InputNameChangedArgs : OBSEventArgs<InputNameChanged.EventDataType> { }
internal sealed class InputSettingsChangedArgs : OBSEventArgs<InputSettingsChanged.EventDataType> { }
internal sealed class InputActiveStateChangedArgs : OBSEventArgs<InputActiveStateChanged.EventDataType> { }
internal sealed class InputShowStateChangedArgs : OBSEventArgs<InputShowStateChanged.EventDataType> { }
internal sealed class InputMuteStateChangedArgs : OBSEventArgs<InputMuteStateChanged.EventDataType> { }
internal sealed class InputVolumeChangedArgs : OBSEventArgs<InputVolumeChanged.EventDataType> { }
internal sealed class InputAudioBalanceChangedArgs : OBSEventArgs<InputAudioBalanceChanged.EventDataType> { }
internal sealed class InputAudioSyncOffsetChangedArgs : OBSEventArgs<InputAudioSyncOffsetChanged.EventDataType> { }
internal sealed class InputAudioTracksChangedArgs : OBSEventArgs<InputAudioTracksChanged.EventDataType> { }
internal sealed class InputAudioMonitorTypeChangedArgs : OBSEventArgs<InputAudioMonitorTypeChanged.EventDataType> { }
internal sealed class InputVolumeMetersArgs : OBSEventArgs<InputVolumeMeters.EventDataType> { }
internal sealed class MediaInputPlaybackStartedArgs : OBSEventArgs<MediaInputPlaybackStarted.EventDataType> { }
internal sealed class MediaInputPlaybackEndedArgs : OBSEventArgs<MediaInputPlaybackEnded.EventDataType> { }
internal sealed class MediaInputActionTriggeredArgs : OBSEventArgs<MediaInputActionTriggered.EventDataType> { }
internal sealed class StreamStateChangedArgs : OBSEventArgs<StreamStateChanged.EventDataType> { }
internal sealed class RecordStateChangedArgs : OBSEventArgs<RecordStateChanged.EventDataType> { }
internal sealed class RecordFileChangedArgs : OBSEventArgs<RecordFileChanged.EventDataType> { }
internal sealed class ReplayBufferStateChangedArgs : OBSEventArgs<ReplayBufferStateChanged.EventDataType> { }
internal sealed class VirtualcamStateChangedArgs : OBSEventArgs<VirtualcamStateChanged.EventDataType> { }
internal sealed class ReplayBufferSavedArgs : OBSEventArgs<ReplayBufferSaved.EventDataType> { }
internal sealed class SceneItemCreatedArgs : OBSEventArgs<SceneItemCreated.EventDataType> { }
internal sealed class SceneItemRemovedArgs : OBSEventArgs<SceneItemRemoved.EventDataType> { }
internal sealed class SceneItemListReindexedArgs : OBSEventArgs<SceneItemListReindexed.EventDataType> { }
internal sealed class SceneItemEnableStateChangedArgs : OBSEventArgs<SceneItemEnableStateChanged.EventDataType> { }
internal sealed class SceneItemLockStateChangedArgs : OBSEventArgs<SceneItemLockStateChanged.EventDataType> { }
internal sealed class SceneItemSelectedArgs : OBSEventArgs<SceneItemSelected.EventDataType> { }
internal sealed class SceneItemTransformChangedArgs : OBSEventArgs<SceneItemTransformChanged.EventDataType> { }
internal sealed class SceneCreatedArgs : OBSEventArgs<SceneCreated.EventDataType> { }
internal sealed class SceneRemovedArgs : OBSEventArgs<SceneRemoved.EventDataType> { }
internal sealed class SceneNameChangedArgs : OBSEventArgs<SceneNameChanged.EventDataType> { }
internal sealed class CurrentProgramSceneChangedArgs : OBSEventArgs<CurrentProgramSceneChanged.EventDataType> { }
internal sealed class CurrentPreviewSceneChangedArgs : OBSEventArgs<CurrentPreviewSceneChanged.EventDataType> { }
internal sealed class SceneListChangedArgs : OBSEventArgs<SceneListChanged.EventDataType> { }
internal sealed class CurrentSceneTransitionChangedArgs : OBSEventArgs<CurrentSceneTransitionChanged.EventDataType> { }
internal sealed class CurrentSceneTransitionDurationChangedArgs : OBSEventArgs<CurrentSceneTransitionDurationChanged.EventDataType> { }
internal sealed class SceneTransitionStartedArgs : OBSEventArgs<SceneTransitionStarted.EventDataType> { }
internal sealed class SceneTransitionEndedArgs : OBSEventArgs<SceneTransitionEnded.EventDataType> { }
internal sealed class SceneTransitionVideoEndedArgs : OBSEventArgs<SceneTransitionVideoEnded.EventDataType> { }
internal sealed class StudioModeStateChangedArgs : OBSEventArgs<StudioModeStateChanged.EventDataType> { }
internal sealed class ScreenshotSavedArgs : OBSEventArgs<ScreenshotSaved.EventDataType> { }
internal sealed class VendorEventArgs : OBSEventArgs<VendorEvent.EventDataType> { }
internal sealed class CustomEventArgs : OBSEventArgs<CustomEvent.EventDataType> { }
