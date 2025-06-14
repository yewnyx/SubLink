using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Platforms;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Event;

namespace xyz.yewnyx.SubLink.OBS.Services;

[PublicAPI]
public sealed class OBSRules : IPlatformRules {
    private OBSService? _service;
    private bool _studioModeEnabled = false;

    internal Func<CurrentSceneCollectionChanging.EventDataType, Task>? OnCurrentSceneCollectionChanging;
    internal Func<CurrentSceneCollectionChanged.EventDataType, Task>? OnCurrentSceneCollectionChanged;
    internal Func<SceneCollectionListChanged.EventDataType, Task>? OnSceneCollectionListChanged;
    internal Func<CurrentProfileChanging.EventDataType, Task>? OnCurrentProfileChanging;
    internal Func<CurrentProfileChanged.EventDataType, Task>? OnCurrentProfileChanged;
    internal Func<ProfileListChanged.EventDataType, Task>? OnProfileListChanged;
    internal Func<SourceFilterListReindexed.EventDataType, Task>? OnSourceFilterListReindexed;
    internal Func<SourceFilterCreated.EventDataType, Task>? OnSourceFilterCreated;
    internal Func<SourceFilterRemoved.EventDataType, Task>? OnSourceFilterRemoved;
    internal Func<SourceFilterNameChanged.EventDataType, Task>? OnSourceFilterNameChanged;
    internal Func<SourceFilterSettingsChanged.EventDataType, Task>? OnSourceFilterSettingsChanged;
    internal Func<SourceFilterEnableStateChanged.EventDataType, Task>? OnSourceFilterEnableStateChanged;
    internal Func<Task>? OnExitStarted;
    internal Func<InputCreated.EventDataType, Task>? OnInputCreated;
    internal Func<InputRemoved.EventDataType, Task>? OnInputRemoved;
    internal Func<InputNameChanged.EventDataType, Task>? OnInputNameChanged;
    internal Func<InputSettingsChanged.EventDataType, Task>? OnInputSettingsChanged;
    internal Func<InputActiveStateChanged.EventDataType, Task>? OnInputActiveStateChanged;
    internal Func<InputShowStateChanged.EventDataType, Task>? OnInputShowStateChanged;
    internal Func<InputMuteStateChanged.EventDataType, Task>? OnInputMuteStateChanged;
    internal Func<InputVolumeChanged.EventDataType, Task>? OnInputVolumeChanged;
    internal Func<InputAudioBalanceChanged.EventDataType, Task>? OnInputAudioBalanceChanged;
    internal Func<InputAudioSyncOffsetChanged.EventDataType, Task>? OnInputAudioSyncOffsetChanged;
    internal Func<InputAudioTracksChanged.EventDataType, Task>? OnInputAudioTracksChanged;
    internal Func<InputAudioMonitorTypeChanged.EventDataType, Task>? OnInputAudioMonitorTypeChanged;
    internal Func<InputVolumeMeters.EventDataType, Task>? OnInputVolumeMeters;
    internal Func<MediaInputPlaybackStarted.EventDataType, Task>? OnMediaInputPlaybackStarted;
    internal Func<MediaInputPlaybackEnded.EventDataType, Task>? OnMediaInputPlaybackEnded;
    internal Func<MediaInputActionTriggered.EventDataType, Task>? OnMediaInputActionTriggered;
    internal Func<StreamStateChanged.EventDataType, Task>? OnStreamStateChanged;
    internal Func<RecordStateChanged.EventDataType, Task>? OnRecordStateChanged;
    internal Func<RecordFileChanged.EventDataType, Task>? OnRecordFileChanged;
    internal Func<ReplayBufferStateChanged.EventDataType, Task>? OnReplayBufferStateChanged;
    internal Func<VirtualcamStateChanged.EventDataType, Task>? OnVirtualcamStateChanged;
    internal Func<ReplayBufferSaved.EventDataType, Task>? OnReplayBufferSaved;
    internal Func<SceneItemCreated.EventDataType, Task>? OnSceneItemCreated;
    internal Func<SceneItemRemoved.EventDataType, Task>? OnSceneItemRemoved;
    internal Func<SceneItemListReindexed.EventDataType, Task>? OnSceneItemListReindexed;
    internal Func<SceneItemEnableStateChanged.EventDataType, Task>? OnSceneItemEnableStateChanged;
    internal Func<SceneItemLockStateChanged.EventDataType, Task>? OnSceneItemLockStateChanged;
    internal Func<SceneItemSelected.EventDataType, Task>? OnSceneItemSelected;
    internal Func<SceneItemTransformChanged.EventDataType, Task>? OnSceneItemTransformChanged;
    internal Func<SceneCreated.EventDataType, Task>? OnSceneCreated;
    internal Func<SceneRemoved.EventDataType, Task>? OnSceneRemoved;
    internal Func<SceneNameChanged.EventDataType, Task>? OnSceneNameChanged;
    internal Func<CurrentProgramSceneChanged.EventDataType, Task>? OnCurrentProgramSceneChanged;
    internal Func<CurrentPreviewSceneChanged.EventDataType, Task>? OnCurrentPreviewSceneChanged;
    internal Func<SceneListChanged.EventDataType, Task>? OnSceneListChanged;
    internal Func<CurrentSceneTransitionChanged.EventDataType, Task>? OnCurrentSceneTransitionChanged;
    internal Func<CurrentSceneTransitionDurationChanged.EventDataType, Task>? OnCurrentSceneTransitionDurationChanged;
    internal Func<SceneTransitionStarted.EventDataType, Task>? OnSceneTransitionStarted;
    internal Func<SceneTransitionEnded.EventDataType, Task>? OnSceneTransitionEnded;
    internal Func<SceneTransitionVideoEnded.EventDataType, Task>? OnSceneTransitionVideoEnded;
    internal Func<StudioModeStateChanged.EventDataType, Task>? OnStudioModeStateChanged;
    internal Func<ScreenshotSaved.EventDataType, Task>? OnScreenshotSaved;
    internal Func<VendorEvent.EventDataType, Task>? OnVendorEvent;
    internal Func<CustomEvent.EventDataType, Task>? OnCustomEvent;

    internal void SetService(OBSService service) {
        _service = service;
        Task.Run(async () => {
            _studioModeEnabled = await _service.GetStudioModeEnabledAsync();
        });
    }

    /* Reacts */
    public void ReactToCurrentSceneCollectionChanging(Func<CurrentSceneCollectionChanging.EventDataType, Task> with) { OnCurrentSceneCollectionChanging = with; }
    public void ReactToCurrentSceneCollectionChanged(Func<CurrentSceneCollectionChanged.EventDataType, Task> with) { OnCurrentSceneCollectionChanged = with; }
    public void ReactToSceneCollectionListChanged(Func<SceneCollectionListChanged.EventDataType, Task> with) { OnSceneCollectionListChanged = with; }
    public void ReactToCurrentProfileChanging(Func<CurrentProfileChanging.EventDataType, Task> with) { OnCurrentProfileChanging = with; }
    public void ReactToCurrentProfileChanged(Func<CurrentProfileChanged.EventDataType, Task> with) { OnCurrentProfileChanged = with; }
    public void ReactToProfileListChanged(Func<ProfileListChanged.EventDataType, Task> with) { OnProfileListChanged = with; }
    public void ReactToSourceFilterListReindexed(Func<SourceFilterListReindexed.EventDataType, Task> with) { OnSourceFilterListReindexed = with; }
    public void ReactToSourceFilterCreated(Func<SourceFilterCreated.EventDataType, Task> with) { OnSourceFilterCreated = with; }
    public void ReactToSourceFilterRemoved(Func<SourceFilterRemoved.EventDataType, Task> with) { OnSourceFilterRemoved = with; }
    public void ReactToSourceFilterNameChanged(Func<SourceFilterNameChanged.EventDataType, Task> with) { OnSourceFilterNameChanged = with; }
    public void ReactToSourceFilterSettingsChanged(Func<SourceFilterSettingsChanged.EventDataType, Task> with) { OnSourceFilterSettingsChanged = with; }
    public void ReactToSourceFilterEnableStateChanged(Func<SourceFilterEnableStateChanged.EventDataType, Task> with) { OnSourceFilterEnableStateChanged = with; }
    public void ReactToExitStarted(Func<Task> with) { OnExitStarted = with; }
    public void ReactToInputCreated(Func<InputCreated.EventDataType, Task> with) { OnInputCreated = with; }
    public void ReactToInputRemoved(Func<InputRemoved.EventDataType, Task> with) { OnInputRemoved = with; }
    public void ReactToInputNameChanged(Func<InputNameChanged.EventDataType, Task> with) { OnInputNameChanged = with; }
    public void ReactToInputSettingsChanged(Func<InputSettingsChanged.EventDataType, Task> with) { OnInputSettingsChanged = with; }
    public void ReactToInputActiveStateChanged(Func<InputActiveStateChanged.EventDataType, Task> with) { OnInputActiveStateChanged = with; }
    public void ReactToInputShowStateChanged(Func<InputShowStateChanged.EventDataType, Task> with) { OnInputShowStateChanged = with; }
    public void ReactToInputMuteStateChanged(Func<InputMuteStateChanged.EventDataType, Task> with) { OnInputMuteStateChanged = with; }
    public void ReactToInputVolumeChanged(Func<InputVolumeChanged.EventDataType, Task> with) { OnInputVolumeChanged = with; }
    public void ReactToInputAudioBalanceChanged(Func<InputAudioBalanceChanged.EventDataType, Task> with) { OnInputAudioBalanceChanged = with; }
    public void ReactToInputAudioSyncOffsetChanged(Func<InputAudioSyncOffsetChanged.EventDataType, Task> with) { OnInputAudioSyncOffsetChanged = with; }
    public void ReactToInputAudioTracksChanged(Func<InputAudioTracksChanged.EventDataType, Task> with) { OnInputAudioTracksChanged = with; }
    public void ReactToInputAudioMonitorTypeChanged(Func<InputAudioMonitorTypeChanged.EventDataType, Task> with) { OnInputAudioMonitorTypeChanged = with; }
    public void ReactToInputVolumeMeters(Func<InputVolumeMeters.EventDataType, Task> with) { OnInputVolumeMeters = with; }
    public void ReactToMediaInputPlaybackStarted(Func<MediaInputPlaybackStarted.EventDataType, Task> with) { OnMediaInputPlaybackStarted = with; }
    public void ReactToMediaInputPlaybackEnded(Func<MediaInputPlaybackEnded.EventDataType, Task> with) { OnMediaInputPlaybackEnded = with; }
    public void ReactToMediaInputActionTriggered(Func<MediaInputActionTriggered.EventDataType, Task> with) { OnMediaInputActionTriggered = with; }
    public void ReactToStreamStateChanged(Func<StreamStateChanged.EventDataType, Task> with) { OnStreamStateChanged = with; }
    public void ReactToRecordStateChanged(Func<RecordStateChanged.EventDataType, Task> with) { OnRecordStateChanged = with; }
    public void ReactToRecordFileChanged(Func<RecordFileChanged.EventDataType, Task> with) { OnRecordFileChanged = with; }
    public void ReactToReplayBufferStateChanged(Func<ReplayBufferStateChanged.EventDataType, Task> with) { OnReplayBufferStateChanged = with; }
    public void ReactToVirtualcamStateChanged(Func<VirtualcamStateChanged.EventDataType, Task> with) { OnVirtualcamStateChanged = with; }
    public void ReactToReplayBufferSaved(Func<ReplayBufferSaved.EventDataType, Task> with) { OnReplayBufferSaved = with; }
    public void ReactToSceneItemCreated(Func<SceneItemCreated.EventDataType, Task> with) { OnSceneItemCreated = with; }
    public void ReactToSceneItemRemoved(Func<SceneItemRemoved.EventDataType, Task> with) { OnSceneItemRemoved = with; }
    public void ReactToSceneItemListReindexed(Func<SceneItemListReindexed.EventDataType, Task> with) { OnSceneItemListReindexed = with; }
    public void ReactToSceneItemEnableStateChanged(Func<SceneItemEnableStateChanged.EventDataType, Task> with) { OnSceneItemEnableStateChanged = with; }
    public void ReactToSceneItemLockStateChanged(Func<SceneItemLockStateChanged.EventDataType, Task> with) { OnSceneItemLockStateChanged = with; }
    public void ReactToSceneItemSelected(Func<SceneItemSelected.EventDataType, Task> with) { OnSceneItemSelected = with; }
    public void ReactToSceneItemTransformChanged(Func<SceneItemTransformChanged.EventDataType, Task> with) { OnSceneItemTransformChanged = with; }
    public void ReactToSceneCreated(Func<SceneCreated.EventDataType, Task> with) { OnSceneCreated = with; }
    public void ReactToSceneRemoved(Func<SceneRemoved.EventDataType, Task> with) { OnSceneRemoved = with; }
    public void ReactToSceneNameChanged(Func<SceneNameChanged.EventDataType, Task> with) { OnSceneNameChanged = with; }
    public void ReactToCurrentProgramSceneChanged(Func<CurrentProgramSceneChanged.EventDataType, Task> with) { OnCurrentProgramSceneChanged = with; }
    public void ReactToCurrentPreviewSceneChanged(Func<CurrentPreviewSceneChanged.EventDataType, Task> with) { OnCurrentPreviewSceneChanged = with; }
    public void ReactToSceneListChanged(Func<SceneListChanged.EventDataType, Task> with) { OnSceneListChanged = with; }
    public void ReactToCurrentSceneTransitionChanged(Func<CurrentSceneTransitionChanged.EventDataType, Task> with) { OnCurrentSceneTransitionChanged = with; }
    public void ReactToCurrentSceneTransitionDurationChanged(Func<CurrentSceneTransitionDurationChanged.EventDataType, Task> with) { OnCurrentSceneTransitionDurationChanged = with; }
    public void ReactToSceneTransitionStarted(Func<SceneTransitionStarted.EventDataType, Task> with) { OnSceneTransitionStarted = with; }
    public void ReactToSceneTransitionEnded(Func<SceneTransitionEnded.EventDataType, Task> with) { OnSceneTransitionEnded = with; }
    public void ReactToSceneTransitionVideoEnded(Func<SceneTransitionVideoEnded.EventDataType, Task> with) { OnSceneTransitionVideoEnded = with; }
    public void ReactToStudioModeStateChanged(Func<StudioModeStateChanged.EventDataType, Task> with) { OnStudioModeStateChanged = with; }
    public void ReactToScreenshotSaved(Func<ScreenshotSaved.EventDataType, Task> with) { OnScreenshotSaved = with; }
    public void ReactToVendorEvent(Func<VendorEvent.EventDataType, Task> with) { OnVendorEvent = with; }
    public void ReactToCustomEvent(Func<CustomEvent.EventDataType, Task> with) { OnCustomEvent = with; }

    /* Actions */
    public async Task SetSourceFilterEnabled(string sourceName, string filterName, bool enabled) {
        if (_service == null || string.IsNullOrEmpty(sourceName) || string.IsNullOrEmpty(filterName)) return;
        await _service.SetSourceFilterEnabledAsync(sourceName, filterName, enabled);
    }

    public async Task<string[]> GetHotkeyList() {
        if (_service == null) return [];
        return await _service.GetHotkeyListAsync();
    }

    public async Task TriggerHotkeyByName(string hotkeyName, string? contextName = null) {
        if (_service == null || string.IsNullOrEmpty(hotkeyName)) return;
        await _service.TriggerHotkeyByNameAsync(hotkeyName, contextName);
    }

    public async Task TriggerHotkeyByKeySequence(string? keyId = null, bool? shift = null, bool? control = null, bool? alt = null, bool? command = null) {
        if (_service == null || (string.IsNullOrEmpty(keyId) && shift == null && control == null && alt == null && command == null)) return;
        await _service.TriggerHotkeyByKeySequenceAsync(keyId, shift, control, alt, command);
    }

    public async Task<bool> GetInputMute(string name) {
        if (_service == null || string.IsNullOrEmpty(name)) return false;
        return await _service.GetInputMuteAsync(name);
    }

    public async Task SetInputMute(string name, bool muted) {
        if (_service == null || string.IsNullOrEmpty(name)) return;
        await _service.SetInputMuteAsync(name, muted);
    }

    public async Task<bool> ToggleInputMute(string name) {
        if (_service == null || string.IsNullOrEmpty(name)) return false;
        return await _service.ToggleInputMuteAsync(name);
    }

    public async Task<InputVolume?> GetInputVolume(string name) {
        if (_service == null || string.IsNullOrEmpty(name)) return null;
        return await _service.GetInputVolumeAsync(name);
    }

    public async Task SetInputVolume(string name, float? multiplier = null, float? db = null) {
        if (_service == null || string.IsNullOrEmpty(name)) return;
        await _service.SetInputVolumeAsync(name, multiplier, db);
    }

    public async Task<int> GetInputAudioSyncOffset(string name) {
        if (_service == null || string.IsNullOrEmpty(name)) return 0;
        return await _service.GetInputAudioSyncOffsetAsync(name);
    }

    public async Task SetInputAudioSyncOffset(string name, int offsetMs) {
        if (_service == null || string.IsNullOrEmpty(name)) return;
        await _service.SetInputAudioSyncOffsetAsync(name, offsetMs);
    }

    public async Task<bool> GetSceneItemEnabled(string name, uint id) {
        if (_service == null || string.IsNullOrEmpty(name)) return false;
        return await _service.GetSceneItemEnabledAsync(name, id);
    }

    public async Task SetSceneItemEnabled(string name, uint id, bool enabled) {
        if (_service == null || string.IsNullOrEmpty(name)) return;
        await _service.SetSceneItemEnabledAsync(name, id, enabled);
    }

    public async Task<string> GetActiveScene() {
        if (_service == null) return string.Empty;
        return await _service.GetActiveSceneAsync();
    }

    public async Task SetActiveScene(string sceneName, string transitionName = "Cut") {
        if (_service == null || string.IsNullOrEmpty(sceneName) || string.IsNullOrEmpty(transitionName)) return;

        var currentTransition = await _service.GetCurrentSceneTransitionAsync();
        await _service.SetCurrentSceneTransitionAsync(transitionName);
        await _service.SetActiveSceneAsync(sceneName);

        if (currentTransition != null)
            await _service.SetCurrentSceneTransitionAsync(currentTransition.Name);
    }

    public async Task<string> GetPreviewScene() {
        if (_service == null || !_studioModeEnabled) return string.Empty;
        return await _service.GetPreviewSceneAsync();
    }

    public async Task SetPreviewScene(string name) {
        if (_service == null || !_studioModeEnabled || string.IsNullOrEmpty(name)) return;
        await _service.SetPreviewSceneAsync(name);
    }

    public async Task<TransitionInfo?> GetCurrentSceneTransition() {
        if (_service == null) return null;
        return await _service.GetCurrentSceneTransitionAsync();
    }

    public async Task SetCurrentSceneTransition(string name) {
        if (_service == null || string.IsNullOrEmpty(name)) return;
        await _service.SetCurrentSceneTransitionAsync(name);
    }

    public async Task TriggerTransition() {
        if (_service == null || !_studioModeEnabled) return;
        await _service.TriggerStudioModeTransitionAsync();
    }

    public async Task<bool> GetStudioModeEnabled() {
        if (_service == null) return false;
        return await _service.GetStudioModeEnabledAsync();
    }
}
