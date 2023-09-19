using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using BuildSoft.VRChat.Osc;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Serilog;

namespace xyz.yewnyx.SubLink;

[UsedImplicitly]
internal sealed partial class StreamPadService
{
    private readonly ILogger _logger;
    // private readonly IHostApplicationLifetime _applicationLifetime;
    // private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<StreamPadSettings> _settingsMonitor;
    private StreamPadSettings _settings;

    private IDisposable _subscription;
    private GraphQLHttpClient _graphQLClient;
    public StreamPadService(ILogger logger,
    // IHostApplicationLifetime applicationLifetime,
    // IServiceScopeFactory serviceScopeFactory,
    IOptionsMonitor<StreamPadSettings> settingsMonitor,
    IServiceProvider serviceProvider)
    {
        Globals.serviceProvider = serviceProvider; // This might conflict with TwitchService doing the same thing

        _logger = logger;
        // _applicationLifetime = applicationLifetime;
        // _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateStreamPadSettings);
        _settings = _settingsMonitor.CurrentValue;
        if (!string.IsNullOrWhiteSpace(_settings.WebSocketUrl) && !string.IsNullOrWhiteSpace(_settings.ChannelId))
        {
            _logger.Information("StreamPad | setting up graphql client.");
            _graphQLClient = new GraphQLHttpClient(o =>
            {
                o.WebSocketEndPoint = new Uri(_settings.WebSocketUrl);
                o.WebSocketProtocol = "graphql-ws";
            }, new NewtonsoftJsonSerializer());
        }
        else
        {
            _logger.Information("StreamPad | settings misconfigured.");
        }

    }
    private void UpdateStreamPadSettings(StreamPadSettings twitchSettings) => _settings = twitchSettings;

    public class StreamPadSubscriptionResult
    {
        public DynamicControllerNamedValue[] dynamicControllerNamedValues { get; set; }

        public class DynamicControllerNamedValue
        {
            public string name { get; set; }
            public float value { get; set; }
        }
    }

    public async Task Start()
    {
        if (null == _graphQLClient)
        {
            _logger.Information("StreamPad | graphql client not set up.");
            return;
        }
        else
        {
            _logger.Information("StreamPad | setting up subscription.");

        }

        _graphQLClient.WebsocketConnectionState.Subscribe(s =>
        {
            _logger.Information($"StreamPad | WebSocketConnectionState:{s}");
        });

        _graphQLClient.WebSocketReceiveErrors.Subscribe(e =>
        {
            // TODO: Do we want to stop app on fail?
            // _applicationLifetime.StopApplication();
            if (e is WebSocketException we)
            {
                _logger.Error($"StreamPad | WebSocketException: {we.Message} (WebSocketError {we.WebSocketErrorCode}, ErrorCode {we.ErrorCode}, NativeErrorCode {we.NativeErrorCode}");
            }
            else
            {
                _logger.Error($"StreamPad | Exception in websocket receive stream: {e.ToString()}");
            }
        });

        var controllerValuesRequest = new GraphQLRequest
        {
            Query = @"
            subscription ($channelId: String) {
                dynamicControllerNamedValues(
                    channelId: $channelId
                    ) {
                        name
                        value
                    }
            }",
            Variables = new
            {
                channelId = _settings.ChannelId
            }
        };

        IObservable<GraphQLResponse<StreamPadSubscriptionResult>> subscriptionStream
            = _graphQLClient.CreateSubscriptionStream<StreamPadSubscriptionResult>(controllerValuesRequest);

        _subscription = subscriptionStream.Subscribe(
            response =>
            {
                if (null != response.Errors && response.Errors.Length > 0)
                {
                    // Only the first error really matters
                    _logger.Error($"StreamPad | Graphql exception: {response.Errors[0].Message}");
                } 
                else  if (null != response.Data && response.Data.dynamicControllerNamedValues != null)
                {
                    foreach (StreamPadSubscriptionResult.DynamicControllerNamedValue namedValue in response.Data.dynamicControllerNamedValues)
                    {
                        _logger.Information($"StreamPad | {namedValue.name}:{namedValue.value}");
                        // TODO: Clamp value between -1/1? 2 Significant digits?
                        OscParameter.SendAvatarParameter(namedValue.name, namedValue.value);
                    }
                } 
                else
                {
                    _logger.Error($"StreamPad | Invalid/No response data in subscription");
                }
            }, 
            exception => _logger.Error($"StreamPad | Subscription exception: {exception}"),
            () => _logger.Warning($"StreamPad | Subscription completed"));
    }
    public async Task Stop()
    {
        if (null != _subscription)
        {
            _subscription.Dispose();
        }
        if (null != _graphQLClient)
        {
            _graphQLClient.Dispose();
        }
    }

}
