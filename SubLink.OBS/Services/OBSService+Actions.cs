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
        outRequestMsg.D.RequestData = new OBSClient.SocketDataTypes.Request.SetCurrentProgramScene() {
            SceneName = sceneName
        };
        await _obs.SendDataAsync(outRequestMsg);
    }
}
