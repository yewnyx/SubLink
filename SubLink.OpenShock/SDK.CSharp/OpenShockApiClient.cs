using System;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using OneOf.Types;
using OpenShock.SDK.CSharp.Errors;
using OpenShock.SDK.CSharp.Models;
using OpenShock.SDK.CSharp.Problems;
using OpenShock.SDK.CSharp.Serialization;
using OpenShock.SDK.CSharp.Utils;

namespace OpenShock.SDK.CSharp;

public sealed class OpenShockApiClient : IOpenShockApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new CustomJsonStringEnumConverter() }
    };


    /// <summary>
    /// Initializes a new instance of the <see cref="OpenShockApiClient"/> class. See parameters' descriptions for more information.
    /// </summary>
    /// <param name="apiClientOptions">Options</param>
    public OpenShockApiClient(ApiClientOptions apiClientOptions)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = apiClientOptions.Server,
            DefaultRequestHeaders =
            {
                { "User-Agent", GetUserAgent(apiClientOptions) },
                { "OpenShockToken", apiClientOptions.Token }
            }
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenShockApiClient"/> class with a custom HttpClient.
    /// You will need set everything yourself.
    /// You probably want to use the other constructor, as this one primarily exists for testing purposes.
    /// </summary>
    /// <param name="httpClient"></param>
    public OpenShockApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<OneOf<Success<ImmutableArray<ResponseHubWithShockers>>, UnauthenticatedError>>
        GetOwnShockers(CancellationToken cancellationToken = default)
    {
        using var ownShockersResponse =
            await _httpClient.GetAsync(OpenShockEndpoints.V1.Shockers.OwnShockers, cancellationToken);
        if (!ownShockersResponse.IsSuccess())
        {
            if (ownShockersResponse.StatusCode == HttpStatusCode.Unauthorized) return new UnauthenticatedError();

            throw new OpenShockApiError("Failed to get own shockers", ownShockersResponse.StatusCode);
        }

        return new Success<ImmutableArray<ResponseHubWithShockers>>(
            await ownShockersResponse.Content
                .ReadBaseResponseAsJsonAsync<ImmutableArray<ResponseHubWithShockers>>(cancellationToken,
                    JsonSerializerOptions));
    }

    /// <inheritdoc />
    public async
        Task<OneOf<Success<LcgResponse>, NotFound, HubOffline, UnauthenticatedError>>
        GetHubGateway(Guid hubId, CancellationToken cancellationToken = default)
    {
        using var gatewayResponse =
            await _httpClient.GetAsync(OpenShockEndpoints.V1.Devices.GetGateway(hubId), cancellationToken);
        if (gatewayResponse.IsSuccess())
        {
            return new Success<LcgResponse>(
                await gatewayResponse.Content.ReadBaseResponseAsJsonAsync<LcgResponse>(cancellationToken,
                    JsonSerializerOptions));
        }

        if (gatewayResponse.StatusCode == HttpStatusCode.Unauthorized) return new UnauthenticatedError();

        if (!gatewayResponse.IsProblem())
            throw new OpenShockApiError("Error from backend is not a problem response", gatewayResponse.StatusCode);

        var problem =
            await gatewayResponse.Content.ReadAsJsonAsync<ProblemDetails>(cancellationToken,
                JsonSerializerOptions);

        return problem.Type switch
        {
            "Hub.NotFound" => new NotFound(),
            "Hub.NotOnline" => new HubOffline(),
            _ => throw new OpenShockApiError($"Unknown problem type [{problem.Type}]", gatewayResponse.StatusCode)
        };
    }

    /// <inheritdoc />
    public async Task<RootResponse> GetRoot(CancellationToken cancellationToken = default)
    {
        using var rootResponse = await _httpClient.GetAsync(OpenShockEndpoints.V1.Root, cancellationToken);
        return await rootResponse.Content.ReadBaseResponseAsJsonAsync<RootResponse>(cancellationToken,
            JsonSerializerOptions);
    }

    /// <inheritdoc />
    public async Task<OneOf<Success<SelfResponse>, UnauthenticatedError>> GetSelf(
        CancellationToken cancellationToken = default)
    {
        using var selfResponse = await _httpClient.GetAsync(OpenShockEndpoints.V1.Users.Self, cancellationToken);

        if (!selfResponse.IsSuccess())
        {
            if (selfResponse.StatusCode == HttpStatusCode.Unauthorized) return new UnauthenticatedError();

            throw new OpenShockApiError("Failed to get user self", selfResponse.StatusCode);
        }

        return new Success<SelfResponse>(
            await selfResponse.Content.ReadBaseResponseAsJsonAsync<SelfResponse>(cancellationToken,
                JsonSerializerOptions));
    }

    public async Task<OneOf<Success, ShockerNotFoundOrNoAccess, ShockerPaused, ShockerNoPermission, UnauthenticatedError>> ControlShocker(ControlRequest controlRequest)
    {
        using var controlResponse =
            await _httpClient.PostAsJsonAsync(OpenShockEndpoints.V2.Shockers.Control, controlRequest);

        if (controlResponse.IsSuccess()) return new Success();

        if (controlResponse.StatusCode == HttpStatusCode.Unauthorized) return new UnauthenticatedError();

        if (!controlResponse.IsProblem())
            throw new OpenShockApiError("Error from backend is not a problem response", controlResponse.StatusCode);

        var problem =
            await controlResponse.Content.ReadAsJsonAsync<ShockerControlProblem>(CancellationToken.None,
                JsonSerializerOptions);

        return problem.Type switch
        {
            "Shocker.Control.NotFound" => new ShockerNotFoundOrNoAccess(problem.ShockerId),
            "Shocker.Control.Paused" => new ShockerPaused(problem.ShockerId),
            "Shocker.Control.NoPermission" => new ShockerNoPermission(problem.ShockerId),
            _ => throw new OpenShockApiError($"Unknown problem type [{problem.Type}]", controlResponse.StatusCode)
        };
    }

    /// <inheritdoc />
    public async Task<OneOf<Success<ResponseHubWithToken>, NotFound, UnauthenticatedError>> GetHub(Guid hubId, CancellationToken cancellationToken = default)
    {
        using var hubResponse = await _httpClient.GetAsync(OpenShockEndpoints.V1.Devices.Get(hubId), cancellationToken);

        if (!hubResponse.IsSuccess())
        {
            if (hubResponse.IsProblem())
            {
                var problem =
                    await hubResponse.Content.ReadAsJsonAsync<ShockerControlProblem>(CancellationToken.None,
                        JsonSerializerOptions);
                
                if (problem.Type == "Hub.NotFound") return new NotFound();
            }
            
            if (hubResponse.StatusCode == HttpStatusCode.Unauthorized) return new UnauthenticatedError();

            throw new OpenShockApiError("Failed to get hub by id", hubResponse.StatusCode);
        }

        return new Success<ResponseHubWithToken>(
            await hubResponse.Content.ReadBaseResponseAsJsonAsync<ResponseHubWithToken>(cancellationToken,
                JsonSerializerOptions));
    }

    /// <inheritdoc />
    public async Task<OneOf<Success<bool>, NotFound>> PauseShocker(Guid shockerId, bool paused, CancellationToken cancellationToken = default)
    {
        using var pauseResponse = await _httpClient.PostAsJsonAsync(
            OpenShockEndpoints.V1.Shockers.Pause(shockerId),
            new PauseRequest{ Pause = paused }, JsonSerializerOptions,
            cancellationToken: cancellationToken);

        if (pauseResponse.IsSuccess())
            return new Success<bool>(
                await pauseResponse.Content.ReadBaseResponseAsJsonAsync<bool>(cancellationToken,
                    JsonSerializerOptions));
        
        if (!pauseResponse.IsProblem()) throw new OpenShockApiError("Failed to pause shocker", pauseResponse.StatusCode);
            
        var problem = await pauseResponse.Content.ReadAsJsonAsync<ProblemDetails>(cancellationToken, JsonSerializerOptions);
        if (problem.Type == "Shocker.NotFound") return new NotFound();

        throw new OpenShockApiError("Failed to pause shocker", problem);
    }

    private static string GetUserAgent(ApiClientOptions options)
    {
        var clientAssembly = typeof(OpenShockApiClient).Assembly;
        var clientVersion = clientAssembly.GetName().Version!;

        string programName;
        Version programVersion;

        if (options.Program == null)
        {
            (programName, programVersion) = UserAgentUtils.GetAssemblyInfo();
        }
        else
        {
            programName = options.Program.Name;
            programVersion = options.Program.Version;
        }

        var runtimeVersion = RuntimeInformation.FrameworkDescription;
        if (string.IsNullOrEmpty(runtimeVersion)) runtimeVersion = "Unknown Runtime";

        return
            $"OpenShock.SDK.CSharp/{clientVersion.Major}.{clientVersion.Minor}.{clientVersion.Build} " +
            $"({runtimeVersion}; {UserAgentUtils.GetOs()};" +
            $" {programName} {programVersion.Major}.{programVersion.Minor}.{programVersion.Build})";
    }
    
}