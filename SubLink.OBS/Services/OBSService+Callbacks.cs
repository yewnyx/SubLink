using System.Threading.Tasks;
using xyz.yewnyx.SubLink.OBS.OBSClient;

namespace xyz.yewnyx.SubLink.OBS.Services;

internal sealed partial class OBSService {
    private void WireCallbacks() {
        _obs.OBSConnected += OnObsConnected;
        _obs.OBSDisconnected += OnObsDisconnected;
        _obs.OBSError += OnObsError;
        _obs.CurrentSceneCollectionChanging += OnCurrentSceneCollectionChanging;
        _obs.CurrentSceneCollectionChanged += OnCurrentSceneCollectionChanged;
        _obs.SceneCollectionListChanged += OnSceneCollectionListChanged;
        _obs.CurrentProfileChanging += OnCurrentProfileChanging;
        _obs.CurrentProfileChanged += OnCurrentProfileChanged;
        _obs.ProfileListChanged += OnProfileListChanged;
        _obs.SourceFilterListReindexed += OnSourceFilterListReindexed;
        _obs.SourceFilterCreated += OnSourceFilterCreated;
        _obs.SourceFilterRemoved += OnSourceFilterRemoved;
        _obs.SourceFilterNameChanged += OnSourceFilterNameChanged;
        _obs.SourceFilterSettingsChanged += OnSourceFilterSettingsChanged;
        _obs.SourceFilterEnableStateChanged += OnSourceFilterEnableStateChanged;
        _obs.ExitStarted += OnExitStarted;
        _obs.InputCreated += OnInputCreated;
        _obs.InputRemoved += OnInputRemoved;
        _obs.InputNameChanged += OnInputNameChanged;
        _obs.InputSettingsChanged += OnInputSettingsChanged;
        _obs.InputActiveStateChanged += OnInputActiveStateChanged;
        _obs.InputShowStateChanged += OnInputShowStateChanged;
        _obs.InputMuteStateChanged += OnInputMuteStateChanged;
        _obs.InputVolumeChanged += OnInputVolumeChanged;
        _obs.InputAudioBalanceChanged += OnInputAudioBalanceChanged;
        _obs.InputAudioSyncOffsetChanged += OnInputAudioSyncOffsetChanged;
        _obs.InputAudioTracksChanged += OnInputAudioTracksChanged;
        _obs.InputAudioMonitorTypeChanged += OnInputAudioMonitorTypeChanged;
        _obs.InputVolumeMeters += OnInputVolumeMeters;
        _obs.MediaInputPlaybackStarted += OnMediaInputPlaybackStarted;
        _obs.MediaInputPlaybackEnded += OnMediaInputPlaybackEnded;
        _obs.MediaInputActionTriggered += OnMediaInputActionTriggered;
        _obs.StreamStateChanged += OnStreamStateChanged;
        _obs.RecordStateChanged += OnRecordStateChanged;
        _obs.RecordFileChanged += OnRecordFileChanged;
        _obs.ReplayBufferStateChanged += OnReplayBufferStateChanged;
        _obs.VirtualcamStateChanged += OnVirtualcamStateChanged;
        _obs.ReplayBufferSaved += OnReplayBufferSaved;
        _obs.SceneItemCreated += OnSceneItemCreated;
        _obs.SceneItemRemoved += OnSceneItemRemoved;
        _obs.SceneItemListReindexed += OnSceneItemListReindexed;
        _obs.SceneItemEnableStateChanged += OnSceneItemEnableStateChanged;
        _obs.SceneItemLockStateChanged += OnSceneItemLockStateChanged;
        _obs.SceneItemSelected += OnSceneItemSelected;
        _obs.SceneItemTransformChanged += OnSceneItemTransformChanged;
        _obs.SceneCreated += OnSceneCreated;
        _obs.SceneRemoved += OnSceneRemoved;
        _obs.SceneNameChanged += OnSceneNameChanged;
        _obs.CurrentProgramSceneChanged += OnCurrentProgramSceneChanged;
        _obs.CurrentPreviewSceneChanged += OnCurrentPreviewSceneChanged;
        _obs.SceneListChanged += OnSceneListChanged;
        _obs.CurrentSceneTransitionChanged += OnCurrentSceneTransitionChanged;
        _obs.CurrentSceneTransitionDurationChanged += OnCurrentSceneTransitionDurationChanged;
        _obs.SceneTransitionStarted += OnSceneTransitionStarted;
        _obs.SceneTransitionEnded += OnSceneTransitionEnded;
        _obs.SceneTransitionVideoEnded += OnSceneTransitionVideoEnded;
        _obs.StudioModeStateChanged += OnStudioModeStateChanged;
        _obs.ScreenshotSaved += OnScreenshotSaved;
        _obs.VendorEvent += OnVendorEvent;
        _obs.CustomEvent += OnCustomEvent;
    }

    private void OnObsConnected(object? sender, System.EventArgs e) =>
        _logger.Information("[{TAG}] Connected to websocket", Platform.PlatformName);

    private void OnObsDisconnected(object? sender, System.EventArgs e) =>
        _logger.Warning("[{TAG}] Disconnected from websocket", Platform.PlatformName);

    private void OnObsError(object? sender, OBSErrorArgs e) =>
        _logger.Error("[{TAG}] Error ocured with the websocket\r\n{Exception}", Platform.PlatformName, e.Exception);

    private void OnCurrentSceneCollectionChanging(object? sender, CurrentSceneCollectionChangingArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentSceneCollectionChanging: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCurrentSceneCollectionChanged(object? sender, CurrentSceneCollectionChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentSceneCollectionChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneCollectionListChanged(object? sender, SceneCollectionListChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneCollectionListChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCurrentProfileChanging(object? sender, CurrentProfileChangingArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentProfileChanging: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCurrentProfileChanged(object? sender, CurrentProfileChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentProfileChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnProfileListChanged(object? sender, ProfileListChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnProfileListChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSourceFilterListReindexed(object? sender, SourceFilterListReindexedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSourceFilterListReindexed: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSourceFilterCreated(object? sender, SourceFilterCreatedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSourceFilterCreated: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSourceFilterRemoved(object? sender, SourceFilterRemovedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSourceFilterRemoved: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSourceFilterNameChanged(object? sender, SourceFilterNameChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSourceFilterNameChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSourceFilterSettingsChanged(object? sender, SourceFilterSettingsChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSourceFilterSettingsChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSourceFilterEnableStateChanged(object? sender, SourceFilterEnableStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSourceFilterEnableStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnExitStarted(object? sender, ExitStartedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnExitStarted: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputCreated(object? sender, InputCreatedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputCreated: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputRemoved(object? sender, InputRemovedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputRemoved: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputNameChanged(object? sender, InputNameChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputNameChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputSettingsChanged(object? sender, InputSettingsChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputSettingsChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputActiveStateChanged(object? sender, InputActiveStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputActiveStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputShowStateChanged(object? sender, InputShowStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputShowStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputMuteStateChanged(object? sender, InputMuteStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputMuteStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputVolumeChanged(object? sender, InputVolumeChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputVolumeChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputAudioBalanceChanged(object? sender, InputAudioBalanceChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputAudioBalanceChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputAudioSyncOffsetChanged(object? sender, InputAudioSyncOffsetChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputAudioSyncOffsetChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputAudioTracksChanged(object? sender, InputAudioTracksChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputAudioTracksChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputAudioMonitorTypeChanged(object? sender, InputAudioMonitorTypeChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputAudioMonitorTypeChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnInputVolumeMeters(object? sender, InputVolumeMetersArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnInputVolumeMeters: { } callback })
                await callback(e.Data);
        });
    }

    private void OnMediaInputPlaybackStarted(object? sender, MediaInputPlaybackStartedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnMediaInputPlaybackStarted: { } callback })
                await callback(e.Data);
        });
    }

    private void OnMediaInputPlaybackEnded(object? sender, MediaInputPlaybackEndedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnMediaInputPlaybackEnded: { } callback })
                await callback(e.Data);
        });
    }

    private void OnMediaInputActionTriggered(object? sender, MediaInputActionTriggeredArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnMediaInputActionTriggered: { } callback })
                await callback(e.Data);
        });
    }

    private void OnStreamStateChanged(object? sender, StreamStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnStreamStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnRecordStateChanged(object? sender, RecordStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnRecordStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnRecordFileChanged(object? sender, RecordFileChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnRecordFileChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnReplayBufferStateChanged(object? sender, ReplayBufferStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnReplayBufferStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnVirtualcamStateChanged(object? sender, VirtualcamStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnVirtualcamStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnReplayBufferSaved(object? sender, ReplayBufferSavedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnReplayBufferSaved: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneItemCreated(object? sender, SceneItemCreatedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneItemCreated: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneItemRemoved(object? sender, SceneItemRemovedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneItemRemoved: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneItemListReindexed(object? sender, SceneItemListReindexedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneItemListReindexed: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneItemEnableStateChanged(object? sender, SceneItemEnableStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneItemEnableStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneItemLockStateChanged(object? sender, SceneItemLockStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneItemLockStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneItemSelected(object? sender, SceneItemSelectedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneItemSelected: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneItemTransformChanged(object? sender, SceneItemTransformChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneItemTransformChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneCreated(object? sender, SceneCreatedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneCreated: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneRemoved(object? sender, SceneRemovedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneRemoved: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneNameChanged(object? sender, SceneNameChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneNameChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCurrentProgramSceneChanged(object? sender, CurrentProgramSceneChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentProgramSceneChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCurrentPreviewSceneChanged(object? sender, CurrentPreviewSceneChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentPreviewSceneChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneListChanged(object? sender, SceneListChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneListChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCurrentSceneTransitionChanged(object? sender, CurrentSceneTransitionChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentSceneTransitionChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCurrentSceneTransitionDurationChanged(object? sender, CurrentSceneTransitionDurationChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCurrentSceneTransitionDurationChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneTransitionStarted(object? sender, SceneTransitionStartedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneTransitionStarted: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneTransitionEnded(object? sender, SceneTransitionEndedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneTransitionEnded: { } callback })
                await callback(e.Data);
        });
    }

    private void OnSceneTransitionVideoEnded(object? sender, SceneTransitionVideoEndedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnSceneTransitionVideoEnded: { } callback })
                await callback(e.Data);
        });
    }

    private void OnStudioModeStateChanged(object? sender, StudioModeStateChangedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnStudioModeStateChanged: { } callback })
                await callback(e.Data);
        });
    }

    private void OnScreenshotSaved(object? sender, ScreenshotSavedArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnScreenshotSaved: { } callback })
                await callback(e.Data);
        });
    }

    private void OnVendorEvent(object? sender, VendorEventArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnVendorEvent: { } callback })
                await callback(e.Data);
        });
    }

    private void OnCustomEvent(object? sender, CustomEventArgs e) {
        Task.Run(async () => {
            if (_rules is OBSRules { OnCustomEvent: { } callback })
                await callback(e.Data);
        });
    }
}
