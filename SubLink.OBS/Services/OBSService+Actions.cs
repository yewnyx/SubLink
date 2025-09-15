using System.Threading.Tasks;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

namespace xyz.yewnyx.SubLink.OBS.Services;

internal sealed partial class OBSService {
    public async Task SetSourceFilterEnabledAsync(string sourceName, string filterName, bool enabled) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetSourceFilterEnabled() {
            SourceName = sourceName,
            FilterName = filterName,
            FilterEnabled = enabled
        };
        await _obs.SendDataAsync(outRequestMsg);
    }
    public async Task<string[]> GetHotkeyListAsync() {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetHotkeyList();
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get the list of hotkeys: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return [];
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetHotkeyList;
        return responseData?.Hotkeys ?? [];
    }

    public async Task TriggerHotkeyByNameAsync(string hotkeyName, string? contextName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.TriggerHotkeyByName() {
            HotkeyName = hotkeyName,
            ContextName = contextName
        };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task TriggerHotkeyByKeySequenceAsync(string? keyId, bool? shift, bool? control, bool? alt, bool? command) {
        OutRequestMsg outRequestMsg = new();
        OBSClient.SocketDataTypes.Request.TriggerHotkeyByKeySequence reqData = new() { KeyId = keyId };

        if (shift != null || control != null || alt != null || command != null)
            reqData.KeyModifiers = new() {
                Shift = shift,
                Control = control,
                Alt = alt,
                Command = command
            };

        outRequestMsg.D.RequestData = reqData;
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<bool> GetInputMuteAsync(string inputName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetInputMute() { InputName = inputName };
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get input mute: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return false;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetInputMute;
        return responseData?.InputMuted ?? false;
    }

    public async Task SetInputMuteAsync(string inputName, bool muted) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetInputMute() {
            InputName = inputName,
            InputMuted = muted
        };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<bool> ToggleInputMuteAsync(string inputName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.ToggleInputMute() { InputName = inputName };
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to toggle input mute: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return false;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.ToggleInputMute;
        return responseData?.InputMuted ?? false;
    }

    public async Task<InputVolume?> GetInputVolumeAsync(string inputName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetInputVolume() { InputName = inputName };
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get input volume: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return null;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetInputVolume;
        return new(responseData?.InputVolumeMul ?? 1f, responseData?.InputVolumeDB ?? 0);
    }

    public async Task SetInputVolumeAsync(string inputName, float? multiplier, float? db) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetInputVolume() {
            InputName = inputName,
            InputVolumeMul = multiplier,
            InputVolumeDB = db
        };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<int> GetInputAudioSyncOffsetAsync(string inputName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetInputAudioSyncOffset() { InputName = inputName };
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get input audio sync offset: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return 0;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetInputAudioSyncOffset;
        return responseData?.InputAudioSyncOffset ?? 0;
    }

    public async Task SetInputAudioSyncOffsetAsync(string inputName, int offsetMs) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetInputAudioSyncOffset() {
            InputName = inputName,
            InputAudioSyncOffset = offsetMs
        };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<bool> GetSceneItemEnabledAsync(string sceneName, uint itemId) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetSceneItemEnabled() {
            SceneName = sceneName,
            SceneItemId = itemId
        };
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get scene item enabled status: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return false;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetSceneItemEnabled;
        return responseData?.SceneItemEnabled ?? false;
    }

    public async Task SetSceneItemEnabledAsync(string sceneName, uint itemId, bool enabled) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetSceneItemEnabled() {
            SceneName = sceneName,
            SceneItemId = itemId,
            SceneItemEnabled = enabled
        };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<string> GetActiveSceneAsync() {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetCurrentProgramScene();
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result)
            return result?.RequestStatus.Comment ?? "Result is NULL";

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetCurrentProgramScene;
        return responseData?.SceneName ?? "Unknown";
    }

    public async Task SetActiveSceneAsync(string sceneName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetCurrentProgramScene() { SceneName = sceneName };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<string> GetPreviewSceneAsync() {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetCurrentPreviewScene();
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get current preview scene: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return string.Empty;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetCurrentPreviewScene;
        return responseData?.SceneName ?? string.Empty;
    }

    public async Task SetPreviewSceneAsync(string sceneName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetCurrentPreviewScene() { SceneName = sceneName };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<TransitionInfo?> GetCurrentSceneTransitionAsync() {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetCurrentSceneTransition();
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get current scene transition: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return null;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetCurrentSceneTransition;
        return new(
            responseData?.TransitionName ?? string.Empty,
            responseData?.TransitionKind ?? string.Empty,
            responseData?.TransitionFixed ?? true,
            responseData?.TransitionConfigurable ?? false,
            responseData?.TransitionDuration
        );
    }

    public async Task SetCurrentSceneTransitionAsync(string transitionName) {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetCurrentSceneTransition() { TransitionName = transitionName };
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task TriggerStudioModeTransitionAsync() {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.TriggerStudioModeTransition();
        await _obs.SendDataAsync(outRequestMsg);
    }

    public async Task<bool> GetStudioModeEnabledAsync() {
        OutRequestMsg outRequestMsg = new();
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.GetStudioModeEnabled();
        var result = await _obs.SendDataAsync(outRequestMsg);

        if (result == null || !result.RequestStatus.Result) {
            _logger.Warning("[{TAG}] Failed to get studio mode enabled state: {Comment}", Platform.PlatformName, result?.RequestStatus.Comment ?? "Result is NULL");
            return false;
        }

        var responseData = result.ResponseData as OBSClient.SocketDataTypes.Response.GetStudioModeEnabled;
        return responseData?.StudioModeEnabled ?? false;
    }
}
