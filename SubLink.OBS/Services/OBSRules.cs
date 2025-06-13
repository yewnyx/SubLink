using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;
using xyz.yewnyx.SubLink.Platforms;

namespace xyz.yewnyx.SubLink.OBS.Services;

[PublicAPI]
public sealed class OBSRules : IPlatformRules {
    private OBSService? _service;
    private bool _studioModeEnabled = false;

    internal Func<InEventMsg.CurrentSceneCollectionChanging, Task>? OnCurrentSceneCollectionChanging;
    internal Func<InEventMsg.CurrentSceneCollectionChanged, Task>? OnCurrentSceneCollectionChanged;
    internal Func<InEventMsg.SceneCollectionListChanged, Task>? OnSceneCollectionListChanged;
    internal Func<InEventMsg.CurrentProfileChanging, Task>? OnCurrentProfileChanging;
    internal Func<InEventMsg.CurrentProfileChanged, Task>? OnCurrentProfileChanged;
    internal Func<InEventMsg.ProfileListChanged, Task>? OnProfileListChanged;
    internal Func<InEventMsg.SourceFilterListReindexed, Task>? OnSourceFilterListReindexed;
    internal Func<InEventMsg.SourceFilterCreated, Task>? OnSourceFilterCreated;
    internal Func<InEventMsg.SourceFilterRemoved, Task>? OnSourceFilterRemoved;
    internal Func<InEventMsg.SourceFilterNameChanged, Task>? OnSourceFilterNameChanged;
    internal Func<InEventMsg.SourceFilterSettingsChanged, Task>? OnSourceFilterSettingsChanged;
    internal Func<InEventMsg.SourceFilterEnableStateChanged, Task>? OnSourceFilterEnableStateChanged;
    internal Func<InEventMsg.ExitStarted, Task>? OnExitStarted;
    internal Func<InEventMsg.InputCreated, Task>? OnInputCreated;
    internal Func<InEventMsg.InputRemoved, Task>? OnInputRemoved;
    internal Func<InEventMsg.InputNameChanged, Task>? OnInputNameChanged;
    internal Func<InEventMsg.InputSettingsChanged, Task>? OnInputSettingsChanged;
    internal Func<InEventMsg.InputActiveStateChanged, Task>? OnInputActiveStateChanged;
    internal Func<InEventMsg.InputShowStateChanged, Task>? OnInputShowStateChanged;
    internal Func<InEventMsg.InputMuteStateChanged, Task>? OnInputMuteStateChanged;
    internal Func<InEventMsg.InputVolumeChanged, Task>? OnInputVolumeChanged;
    internal Func<InEventMsg.InputAudioBalanceChanged, Task>? OnInputAudioBalanceChanged;
    internal Func<InEventMsg.InputAudioSyncOffsetChanged, Task>? OnInputAudioSyncOffsetChanged;
    internal Func<InEventMsg.InputAudioTracksChanged, Task>? OnInputAudioTracksChanged;
    internal Func<InEventMsg.InputAudioMonitorTypeChanged, Task>? OnInputAudioMonitorTypeChanged;
    internal Func<InEventMsg.InputVolumeMeters, Task>? OnInputVolumeMeters;
    internal Func<InEventMsg.MediaInputPlaybackStarted, Task>? OnMediaInputPlaybackStarted;
    internal Func<InEventMsg.MediaInputPlaybackEnded, Task>? OnMediaInputPlaybackEnded;
    internal Func<InEventMsg.MediaInputActionTriggered, Task>? OnMediaInputActionTriggered;
    internal Func<InEventMsg.StreamStateChanged, Task>? OnStreamStateChanged;
    internal Func<InEventMsg.RecordStateChanged, Task>? OnRecordStateChanged;
    internal Func<InEventMsg.RecordFileChanged, Task>? OnRecordFileChanged;
    internal Func<InEventMsg.ReplayBufferStateChanged, Task>? OnReplayBufferStateChanged;
    internal Func<InEventMsg.VirtualcamStateChanged, Task>? OnVirtualcamStateChanged;
    internal Func<InEventMsg.ReplayBufferSaved, Task>? OnReplayBufferSaved;
    internal Func<InEventMsg.SceneItemCreated, Task>? OnSceneItemCreated;
    internal Func<InEventMsg.SceneItemRemoved, Task>? OnSceneItemRemoved;
    internal Func<InEventMsg.SceneItemListReindexed, Task>? OnSceneItemListReindexed;
    internal Func<InEventMsg.SceneItemEnableStateChanged, Task>? OnSceneItemEnableStateChanged;
    internal Func<InEventMsg.SceneItemLockStateChanged, Task>? OnSceneItemLockStateChanged;
    internal Func<InEventMsg.SceneItemSelected, Task>? OnSceneItemSelected;
    internal Func<InEventMsg.SceneItemTransformChanged, Task>? OnSceneItemTransformChanged;
    internal Func<InEventMsg.SceneCreated, Task>? OnSceneCreated;
    internal Func<InEventMsg.SceneRemoved, Task>? OnSceneRemoved;
    internal Func<InEventMsg.SceneNameChanged, Task>? OnSceneNameChanged;
    internal Func<InEventMsg.CurrentProgramSceneChanged, Task>? OnCurrentProgramSceneChanged;
    internal Func<InEventMsg.CurrentPreviewSceneChanged, Task>? OnCurrentPreviewSceneChanged;
    internal Func<InEventMsg.SceneListChanged, Task>? OnSceneListChanged;
    internal Func<InEventMsg.CurrentSceneTransitionChanged, Task>? OnCurrentSceneTransitionChanged;
    internal Func<InEventMsg.CurrentSceneTransitionDurationChanged, Task>? OnCurrentSceneTransitionDurationChanged;
    internal Func<InEventMsg.SceneTransitionStarted, Task>? OnSceneTransitionStarted;
    internal Func<InEventMsg.SceneTransitionEnded, Task>? OnSceneTransitionEnded;
    internal Func<InEventMsg.SceneTransitionVideoEnded, Task>? OnSceneTransitionVideoEnded;
    internal Func<InEventMsg.StudioModeStateChanged, Task>? OnStudioModeStateChanged;
    internal Func<InEventMsg.ScreenshotSaved, Task>? OnScreenshotSaved;
    internal Func<InEventMsg.VendorEvent, Task>? OnVendorEvent;
    internal Func<InEventMsg.CustomEvent, Task>? OnCustomEvent;

    internal void SetService(OBSService service) {
        _service = service;
        Task.Run(async () => {
            _studioModeEnabled = await _service.GetStudioModeEnabledAsync();
        });
    }

    /* Reacts */
    public void ReactToCurrentSceneCollectionChanging(Func<InEventMsg.CurrentSceneCollectionChanging, Task> with) { OnCurrentSceneCollectionChanging = with; }
    public void ReactToCurrentSceneCollectionChanged(Func<InEventMsg.CurrentSceneCollectionChanged, Task> with) { OnCurrentSceneCollectionChanged = with; }
    public void ReactToSceneCollectionListChanged(Func<InEventMsg.SceneCollectionListChanged, Task> with) { OnSceneCollectionListChanged = with; }
    public void ReactToCurrentProfileChanging(Func<InEventMsg.CurrentProfileChanging, Task> with) { OnCurrentProfileChanging = with; }
    public void ReactToCurrentProfileChanged(Func<InEventMsg.CurrentProfileChanged, Task> with) { OnCurrentProfileChanged = with; }
    public void ReactToProfileListChanged(Func<InEventMsg.ProfileListChanged, Task> with) { OnProfileListChanged = with; }
    public void ReactToSourceFilterListReindexed(Func<InEventMsg.SourceFilterListReindexed, Task> with) { OnSourceFilterListReindexed = with; }
    public void ReactToSourceFilterCreated(Func<InEventMsg.SourceFilterCreated, Task> with) { OnSourceFilterCreated = with; }
    public void ReactToSourceFilterRemoved(Func<InEventMsg.SourceFilterRemoved, Task> with) { OnSourceFilterRemoved = with; }
    public void ReactToSourceFilterNameChanged(Func<InEventMsg.SourceFilterNameChanged, Task> with) { OnSourceFilterNameChanged = with; }
    public void ReactToSourceFilterSettingsChanged(Func<InEventMsg.SourceFilterSettingsChanged, Task> with) { OnSourceFilterSettingsChanged = with; }
    public void ReactToSourceFilterEnableStateChanged(Func<InEventMsg.SourceFilterEnableStateChanged, Task> with) { OnSourceFilterEnableStateChanged = with; }
    public void ReactToExitStarted(Func<InEventMsg.ExitStarted, Task> with) { OnExitStarted = with; }
    public void ReactToInputCreated(Func<InEventMsg.InputCreated, Task> with) { OnInputCreated = with; }
    public void ReactToInputRemoved(Func<InEventMsg.InputRemoved, Task> with) { OnInputRemoved = with; }
    public void ReactToInputNameChanged(Func<InEventMsg.InputNameChanged, Task> with) { OnInputNameChanged = with; }
    public void ReactToInputSettingsChanged(Func<InEventMsg.InputSettingsChanged, Task> with) { OnInputSettingsChanged = with; }
    public void ReactToInputActiveStateChanged(Func<InEventMsg.InputActiveStateChanged, Task> with) { OnInputActiveStateChanged = with; }
    public void ReactToInputShowStateChanged(Func<InEventMsg.InputShowStateChanged, Task> with) { OnInputShowStateChanged = with; }
    public void ReactToInputMuteStateChanged(Func<InEventMsg.InputMuteStateChanged, Task> with) { OnInputMuteStateChanged = with; }
    public void ReactToInputVolumeChanged(Func<InEventMsg.InputVolumeChanged, Task> with) { OnInputVolumeChanged = with; }
    public void ReactToInputAudioBalanceChanged(Func<InEventMsg.InputAudioBalanceChanged, Task> with) { OnInputAudioBalanceChanged = with; }
    public void ReactToInputAudioSyncOffsetChanged(Func<InEventMsg.InputAudioSyncOffsetChanged, Task> with) { OnInputAudioSyncOffsetChanged = with; }
    public void ReactToInputAudioTracksChanged(Func<InEventMsg.InputAudioTracksChanged, Task> with) { OnInputAudioTracksChanged = with; }
    public void ReactToInputAudioMonitorTypeChanged(Func<InEventMsg.InputAudioMonitorTypeChanged, Task> with) { OnInputAudioMonitorTypeChanged = with; }
    public void ReactToInputVolumeMeters(Func<InEventMsg.InputVolumeMeters, Task> with) { OnInputVolumeMeters = with; }
    public void ReactToMediaInputPlaybackStarted(Func<InEventMsg.MediaInputPlaybackStarted, Task> with) { OnMediaInputPlaybackStarted = with; }
    public void ReactToMediaInputPlaybackEnded(Func<InEventMsg.MediaInputPlaybackEnded, Task> with) { OnMediaInputPlaybackEnded = with; }
    public void ReactToMediaInputActionTriggered(Func<InEventMsg.MediaInputActionTriggered, Task> with) { OnMediaInputActionTriggered = with; }
    public void ReactToStreamStateChanged(Func<InEventMsg.StreamStateChanged, Task> with) { OnStreamStateChanged = with; }
    public void ReactToRecordStateChanged(Func<InEventMsg.RecordStateChanged, Task> with) { OnRecordStateChanged = with; }
    public void ReactToRecordFileChanged(Func<InEventMsg.RecordFileChanged, Task> with) { OnRecordFileChanged = with; }
    public void ReactToReplayBufferStateChanged(Func<InEventMsg.ReplayBufferStateChanged, Task> with) { OnReplayBufferStateChanged = with; }
    public void ReactToVirtualcamStateChanged(Func<InEventMsg.VirtualcamStateChanged, Task> with) { OnVirtualcamStateChanged = with; }
    public void ReactToReplayBufferSaved(Func<InEventMsg.ReplayBufferSaved, Task> with) { OnReplayBufferSaved = with; }
    public void ReactToSceneItemCreated(Func<InEventMsg.SceneItemCreated, Task> with) { OnSceneItemCreated = with; }
    public void ReactToSceneItemRemoved(Func<InEventMsg.SceneItemRemoved, Task> with) { OnSceneItemRemoved = with; }
    public void ReactToSceneItemListReindexed(Func<InEventMsg.SceneItemListReindexed, Task> with) { OnSceneItemListReindexed = with; }
    public void ReactToSceneItemEnableStateChanged(Func<InEventMsg.SceneItemEnableStateChanged, Task> with) { OnSceneItemEnableStateChanged = with; }
    public void ReactToSceneItemLockStateChanged(Func<InEventMsg.SceneItemLockStateChanged, Task> with) { OnSceneItemLockStateChanged = with; }
    public void ReactToSceneItemSelected(Func<InEventMsg.SceneItemSelected, Task> with) { OnSceneItemSelected = with; }
    public void ReactToSceneItemTransformChanged(Func<InEventMsg.SceneItemTransformChanged, Task> with) { OnSceneItemTransformChanged = with; }
    public void ReactToSceneCreated(Func<InEventMsg.SceneCreated, Task> with) { OnSceneCreated = with; }
    public void ReactToSceneRemoved(Func<InEventMsg.SceneRemoved, Task> with) { OnSceneRemoved = with; }
    public void ReactToSceneNameChanged(Func<InEventMsg.SceneNameChanged, Task> with) { OnSceneNameChanged = with; }
    public void ReactToCurrentProgramSceneChanged(Func<InEventMsg.CurrentProgramSceneChanged, Task> with) { OnCurrentProgramSceneChanged = with; }
    public void ReactToCurrentPreviewSceneChanged(Func<InEventMsg.CurrentPreviewSceneChanged, Task> with) { OnCurrentPreviewSceneChanged = with; }
    public void ReactToSceneListChanged(Func<InEventMsg.SceneListChanged, Task> with) { OnSceneListChanged = with; }
    public void ReactToCurrentSceneTransitionChanged(Func<InEventMsg.CurrentSceneTransitionChanged, Task> with) { OnCurrentSceneTransitionChanged = with; }
    public void ReactToCurrentSceneTransitionDurationChanged(Func<InEventMsg.CurrentSceneTransitionDurationChanged, Task> with) { OnCurrentSceneTransitionDurationChanged = with; }
    public void ReactToSceneTransitionStarted(Func<InEventMsg.SceneTransitionStarted, Task> with) { OnSceneTransitionStarted = with; }
    public void ReactToSceneTransitionEnded(Func<InEventMsg.SceneTransitionEnded, Task> with) { OnSceneTransitionEnded = with; }
    public void ReactToSceneTransitionVideoEnded(Func<InEventMsg.SceneTransitionVideoEnded, Task> with) { OnSceneTransitionVideoEnded = with; }
    public void ReactToStudioModeStateChanged(Func<InEventMsg.StudioModeStateChanged, Task> with) { OnStudioModeStateChanged = with; }
    public void ReactToScreenshotSaved(Func<InEventMsg.ScreenshotSaved, Task> with) { OnScreenshotSaved = with; }
    public void ReactToVendorEvent(Func<InEventMsg.VendorEvent, Task> with) { OnVendorEvent = with; }
    public void ReactToCustomEvent(Func<InEventMsg.CustomEvent, Task> with) { OnCustomEvent = with; }

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
        if (_service == null || string.IsNullOrEmpty(sceneName)) return;

        if (_studioModeEnabled) {
            var currentTransition = await _service.GetCurrentSceneTransitionAsync();
            await _service.SetCurrentSceneTransitionAsync(transitionName);
            await _service.SetActiveSceneAsync(sceneName);

            if (currentTransition != null)
                await _service.SetCurrentSceneTransitionAsync(currentTransition.Name);
        } else {
            await _service.SetActiveSceneAsync(sceneName);
        }
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
