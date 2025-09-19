using JetBrains.Annotations;
using xyz.yewnyx.SubLink.Platforms;
using System.Threading.Tasks;
using System.Collections.Immutable;
using OpenShock.SDK.CSharp.Models;

namespace xyz.yewnyx.SubLink.OpenShock.Services;

[PublicAPI]
public sealed class OpenShockRules : IPlatformRules {
    private OpenShockService? _service;

    internal void SetService(OpenShockService service) =>
        _service = service;

    /* Actions */
    public async Task<bool> VibrateShocker(string shockerId, byte intensity, ushort duration, bool exclusive = true) {
        if (_service == null || string.IsNullOrEmpty(shockerId) || intensity > 100 || duration < 300 || duration > 30000) return false;
        return await _service.VibrateShockerAsync(shockerId, intensity, duration, exclusive);
    }

    public async Task<bool> SoundShocker(string shockerId, byte intensity, ushort duration, bool exclusive = true) {
        if (_service == null || string.IsNullOrEmpty(shockerId) || intensity > 100 || duration < 300 || duration > 30000) return false;
        return await _service.SoundShockerAsync(shockerId, intensity, duration, exclusive);
    }

    public async Task<bool> ShockShocker(string shockerId, byte intensity, ushort duration, bool exclusive = true) {
        if (_service == null || string.IsNullOrEmpty(shockerId) || intensity > 100 || duration < 300 || duration > 30000) return false;
        return await _service.ShockShockerAsync(shockerId, intensity, duration, exclusive);
    }

    public async Task<bool> StopShocker(string shockerId) {
        if (_service == null || string.IsNullOrEmpty(shockerId)) return false;
        return await _service.StopShockerAsync(shockerId);
    }

    public async Task<ImmutableArray<ResponseHubWithShockers>> GetOwnShockers() {
        if (_service == null) return [];
        return await _service.GetOwnShockersAsync();
    }

    public async Task<bool> PauseShocker(string shockerId) {
        if (_service == null || string.IsNullOrEmpty(shockerId)) return false;
        return await _service.PauseShockerAsync(shockerId);
    }

    public async Task<bool> ResumeShocker(string shockerId) {
        if (_service == null || string.IsNullOrEmpty(shockerId)) return false;
        return await _service.ResumeShockerAsync(shockerId);
    }
}
