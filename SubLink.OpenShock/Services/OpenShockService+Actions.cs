using OpenShock.SDK.CSharp.Models;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink.OpenShock.Services;

internal sealed partial class OpenShockService {
    public async Task<bool> VibrateShockerAsync(string shockerId, byte intensity, ushort duration, bool exclusive = true) {
        if (_openShock == null) return false;

        var response = await _openShock.ControlShocker(new() {
            CustomName = null,
            Shocks = [new() {
                Id = new(shockerId),
                Type = ControlType.Vibrate,
                Intensity = intensity,
                Duration = duration,
                Exclusive = exclusive
            }]
        });
        response.Switch(
            success => { },
            notFoundOrNoAccess => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` not found or no access to shocker", Platform.PlatformName, shockerId);
            },
            paused => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` is paused", Platform.PlatformName, shockerId);
            },
            noPerm => {
                _logger.Warning("[{TAG}] No permission to control `{ShockerId}`", Platform.PlatformName, shockerId);
            },
            unauthenticated => {
                _logger.Warning("[{TAG}] Not authenticated", Platform.PlatformName);
            }
        );
        return response.IsT0;
    }

    public async Task<bool> SoundShockerAsync(string shockerId, byte intensity, ushort duration, bool exclusive = true) {
        if (_openShock == null) return false;

        var response = await _openShock.ControlShocker(new() {
            CustomName = null,
            Shocks = [new() {
                Id = new(shockerId),
                Type = ControlType.Sound,
                Intensity = intensity,
                Duration = duration,
                Exclusive = exclusive
            }]
        });
        response.Switch(
            success => { },
            notFoundOrNoAccess => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` not found or no access to shocker", Platform.PlatformName, shockerId);
            },
            paused => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` is paused", Platform.PlatformName, shockerId);
            },
            noPerm => {
                _logger.Warning("[{TAG}] No permission to control `{ShockerId}`", Platform.PlatformName, shockerId);
            },
            unauthenticated => {
                _logger.Warning("[{TAG}] Not authenticated", Platform.PlatformName);
            }
        );
        return response.IsT0;
    }

    public async Task<bool> ShockShockerAsync(string shockerId, byte intensity, ushort duration, bool exclusive = true) {
        if (_openShock == null) return false;

        var response = await _openShock.ControlShocker(new() {
            CustomName = null,
            Shocks = [new() {
                Id = new(shockerId),
                Type = ControlType.Shock,
                Intensity = intensity,
                Duration = duration,
                Exclusive = exclusive
            }]
        });
        response.Switch(
            success => { },
            notFoundOrNoAccess => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` not found or no access to shocker", Platform.PlatformName, shockerId);
            },
            paused => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` is paused", Platform.PlatformName, shockerId);
            },
            noPerm => {
                _logger.Warning("[{TAG}] No permission to control `{ShockerId}`", Platform.PlatformName, shockerId);
            },
            unauthenticated => {
                _logger.Warning("[{TAG}] Not authenticated", Platform.PlatformName);
            }
        );
        return response.IsT0;
    }

    public async Task<bool> StopShockerAsync(string shockerId) {
        if (_openShock == null) return false;

        var response = await _openShock.ControlShocker(new() {
            CustomName = null,
            Shocks = [new() {
                Id = new(shockerId),
                Type = ControlType.Stop,
                Intensity = 0,
                Duration = 300
            }]
        });
        response.Switch(
            success => { },
            notFoundOrNoAccess => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` not found or no access to shocker", Platform.PlatformName, shockerId);
            },
            paused => {
                _logger.Warning("[{TAG}] Shocker `{ShockerId}` is paused", Platform.PlatformName, shockerId);
            },
            noPerm => {
                _logger.Warning("[{TAG}] No permission to control `{ShockerId}`", Platform.PlatformName, shockerId);
            },
            unauthenticated => {
                _logger.Warning("[{TAG}] Not authenticated", Platform.PlatformName);
            }
        );
        return response.IsT0;
    }

    public async Task<ImmutableArray<ResponseHubWithShockers>> GetOwnShockersAsync() {
        if (_openShock == null) return [];

        var response = await _openShock.GetOwnShockers();
        return response.IsT0 ? response.AsT0.Value : [];
    }

    public async Task<bool> PauseShockerAsync(string shockerId) {
        if (_openShock == null) return false;

        var response = await _openShock.PauseShocker(new(shockerId), true);
        return response.IsT0;
    }

    public async Task<bool> ResumeShockerAsync(string shockerId) {
        if (_openShock == null) return false;

        var response = await _openShock.PauseShocker(new(shockerId), false);
        return response.IsT0;
    }
}
