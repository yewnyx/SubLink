using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using OneOf.Types;
using OpenShock.SDK.CSharp.Errors;
using OpenShock.SDK.CSharp.Models;

namespace OpenShock.SDK.CSharp;

public interface IOpenShockApiClient
{
    /// <summary>
    /// Get own shockers with hubs
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<OneOf<Success<ImmutableArray<ResponseHubWithShockers>>, UnauthenticatedError>> GetOwnShockers(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the gateway a hub is connected to
    /// </summary>
    /// <param name="hubId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<OneOf<Success<LcgResponse>, NotFound, HubOffline, UnauthenticatedError>> GetHubGateway(Guid hubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the root for the API, this has some useful information and can be used to check if the API is reachable
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<RootResponse> GetRoot(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get user's information
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<OneOf<Success<SelfResponse>, UnauthenticatedError>> GetSelf(CancellationToken cancellationToken = default);

    /// <summary>
    /// Control a shocker
    /// </summary>
    /// <param name="controlRequest"></param>
    /// <returns></returns>
    public Task<OneOf<Success, ShockerNotFoundOrNoAccess, ShockerPaused, ShockerNoPermission, UnauthenticatedError>> ControlShocker(ControlRequest controlRequest);

    /// <summary>
    /// Get a hub with its token if you have permissions
    /// </summary>
    /// <param name="hubId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<OneOf<Success<ResponseHubWithToken>, NotFound, UnauthenticatedError>> GetHub(Guid hubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pause or unpause a shocker
    /// </summary>
    /// <param name="shockerId"></param>
    /// <param name="paused">True when the shocker needs to be paused</param>
    /// <param name="cancellationToken"></param>
    /// <returns>bool that indicates the current state of the shocker pause</returns>
    public Task<OneOf<Success<bool>, NotFound>> PauseShocker(Guid shockerId, bool paused,
        CancellationToken cancellationToken = default);
}

public struct HubOffline;

