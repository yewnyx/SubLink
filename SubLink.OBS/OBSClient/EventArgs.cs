using System;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

namespace xyz.yewnyx.SubLink.OBS.OBSClient;

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

internal sealed class CurrentSceneCollectionChangingArgs : OBSEventArgs<InEventMsg.CurrentSceneCollectionChanging> { }
internal sealed class CurrentSceneCollectionChangedArgs : OBSEventArgs<InEventMsg.CurrentSceneCollectionChanged> { }
internal sealed class SceneCollectionListChangedArgs : OBSEventArgs<InEventMsg.SceneCollectionListChanged> { }
internal sealed class CurrentProfileChangingArgs : OBSEventArgs<InEventMsg.CurrentProfileChanging> { }
internal sealed class CurrentProfileChangedArgs : OBSEventArgs<InEventMsg.CurrentProfileChanged> { }
internal sealed class ProfileListChangedArgs : OBSEventArgs<InEventMsg.ProfileListChanged> { }
internal sealed class SourceFilterListReindexedArgs : OBSEventArgs<InEventMsg.SourceFilterListReindexed> { }
internal sealed class SourceFilterCreatedArgs : OBSEventArgs<InEventMsg.SourceFilterCreated> { }
internal sealed class SourceFilterRemovedArgs : OBSEventArgs<InEventMsg.SourceFilterRemoved> { }
internal sealed class SourceFilterNameChangedArgs : OBSEventArgs<InEventMsg.SourceFilterNameChanged> { }
internal sealed class SourceFilterSettingsChangedArgs : OBSEventArgs<InEventMsg.SourceFilterSettingsChanged> { }
internal sealed class SourceFilterEnableStateChangedArgs : OBSEventArgs<InEventMsg.SourceFilterEnableStateChanged> { }
internal sealed class ExitStartedArgs : OBSEventArgs<InEventMsg.ExitStarted> { }
internal sealed class InputCreatedArgs : OBSEventArgs<InEventMsg.InputCreated> { }
internal sealed class InputRemovedArgs : OBSEventArgs<InEventMsg.InputRemoved> { }
internal sealed class InputNameChangedArgs : OBSEventArgs<InEventMsg.InputNameChanged> { }
internal sealed class InputSettingsChangedArgs : OBSEventArgs<InEventMsg.InputSettingsChanged> { }
internal sealed class InputActiveStateChangedArgs : OBSEventArgs<InEventMsg.InputActiveStateChanged> { }
internal sealed class InputShowStateChangedArgs : OBSEventArgs<InEventMsg.InputShowStateChanged> { }
internal sealed class InputMuteStateChangedArgs : OBSEventArgs<InEventMsg.InputMuteStateChanged> { }
internal sealed class InputVolumeChangedArgs : OBSEventArgs<InEventMsg.InputVolumeChanged> { }
internal sealed class InputAudioBalanceChangedArgs : OBSEventArgs<InEventMsg.InputAudioBalanceChanged> { }
internal sealed class InputAudioSyncOffsetChangedArgs : OBSEventArgs<InEventMsg.InputAudioSyncOffsetChanged> { }
internal sealed class InputAudioTracksChangedArgs : OBSEventArgs<InEventMsg.InputAudioTracksChanged> { }
internal sealed class InputAudioMonitorTypeChangedArgs : OBSEventArgs<InEventMsg.InputAudioMonitorTypeChanged> { }
internal sealed class InputVolumeMetersArgs : OBSEventArgs<InEventMsg.InputVolumeMeters> { }
internal sealed class MediaInputPlaybackStartedArgs : OBSEventArgs<InEventMsg.MediaInputPlaybackStarted> { }
internal sealed class MediaInputPlaybackEndedArgs : OBSEventArgs<InEventMsg.MediaInputPlaybackEnded> { }
internal sealed class MediaInputActionTriggeredArgs : OBSEventArgs<InEventMsg.MediaInputActionTriggered> { }
internal sealed class StreamStateChangedArgs : OBSEventArgs<InEventMsg.StreamStateChanged> { }
internal sealed class RecordStateChangedArgs : OBSEventArgs<InEventMsg.RecordStateChanged> { }
internal sealed class RecordFileChangedArgs : OBSEventArgs<InEventMsg.RecordFileChanged> { }
internal sealed class ReplayBufferStateChangedArgs : OBSEventArgs<InEventMsg.ReplayBufferStateChanged> { }
internal sealed class VirtualcamStateChangedArgs : OBSEventArgs<InEventMsg.VirtualcamStateChanged> { }
internal sealed class ReplayBufferSavedArgs : OBSEventArgs<InEventMsg.ReplayBufferSaved> { }
internal sealed class SceneItemCreatedArgs : OBSEventArgs<InEventMsg.SceneItemCreated> { }
internal sealed class SceneItemRemovedArgs : OBSEventArgs<InEventMsg.SceneItemRemoved> { }
internal sealed class SceneItemListReindexedArgs : OBSEventArgs<InEventMsg.SceneItemListReindexed> { }
internal sealed class SceneItemEnableStateChangedArgs : OBSEventArgs<InEventMsg.SceneItemEnableStateChanged> { }
internal sealed class SceneItemLockStateChangedArgs : OBSEventArgs<InEventMsg.SceneItemLockStateChanged> { }
internal sealed class SceneItemSelectedArgs : OBSEventArgs<InEventMsg.SceneItemSelected> { }
internal sealed class SceneItemTransformChangedArgs : OBSEventArgs<InEventMsg.SceneItemTransformChanged> { }
internal sealed class SceneCreatedArgs : OBSEventArgs<InEventMsg.SceneCreated> { }
internal sealed class SceneRemovedArgs : OBSEventArgs<InEventMsg.SceneRemoved> { }
internal sealed class SceneNameChangedArgs : OBSEventArgs<InEventMsg.SceneNameChanged> { }
internal sealed class CurrentProgramSceneChangedArgs : OBSEventArgs<InEventMsg.CurrentProgramSceneChanged> { }
internal sealed class CurrentPreviewSceneChangedArgs : OBSEventArgs<InEventMsg.CurrentPreviewSceneChanged> { }
internal sealed class SceneListChangedArgs : OBSEventArgs<InEventMsg.SceneListChanged> { }
internal sealed class CurrentSceneTransitionChangedArgs : OBSEventArgs<InEventMsg.CurrentSceneTransitionChanged> { }
internal sealed class CurrentSceneTransitionDurationChangedArgs : OBSEventArgs<InEventMsg.CurrentSceneTransitionDurationChanged> { }
internal sealed class SceneTransitionStartedArgs : OBSEventArgs<InEventMsg.SceneTransitionStarted> { }
internal sealed class SceneTransitionEndedArgs : OBSEventArgs<InEventMsg.SceneTransitionEnded> { }
internal sealed class SceneTransitionVideoEndedArgs : OBSEventArgs<InEventMsg.SceneTransitionVideoEnded> { }
internal sealed class StudioModeStateChangedArgs : OBSEventArgs<InEventMsg.StudioModeStateChanged> { }
internal sealed class ScreenshotSavedArgs : OBSEventArgs<InEventMsg.ScreenshotSaved> { }
internal sealed class VendorEventArgs : OBSEventArgs<InEventMsg.VendorEvent> { }
internal sealed class CustomEventArgs : OBSEventArgs<InEventMsg.CustomEvent> { }
