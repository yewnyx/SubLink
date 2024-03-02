using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Serilog;

namespace xyz.yewnyx.SubLink.Streampad.Services;

[UsedImplicitly]
internal sealed class StreamPadService {
    private readonly ILogger _logger;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<StreamPadSettings> _settingsMonitor;
    private StreamPadSettings _settings;

    private readonly StreamPadRules _rules;

    private IDisposable? _subscription;
    private readonly GraphQLHttpClient? _graphQLClient;

    public StreamPadService(
        ILogger logger,
        IOptionsMonitor<StreamPadSettings> settingsMonitor,
        StreamPadRules rules) {
        _logger = logger;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateStreampadSettings);
        _settings = _settingsMonitor.CurrentValue;

        _rules = rules;

        if (!string.IsNullOrWhiteSpace(_settings.WebSocketUrl) && !string.IsNullOrWhiteSpace(_settings.ChannelId)) {
            _logger.Information("[{TAG}] setting up graphql client.", Platform.PlatformName);
            _graphQLClient = new GraphQLHttpClient(o => {
                o.WebSocketEndPoint = new Uri(_settings.WebSocketUrl);
                o.WebSocketProtocol = "graphql-ws";
            }, new NewtonsoftJsonSerializer());
        } else {
            _logger.Information("[{TAG}] settings misconfigured.", Platform.PlatformName);
        }
    }

    private void UpdateStreampadSettings(StreamPadSettings settings) => _settings = settings;

#pragma warning disable IDE1006 // Naming Styles
    public class StreamPadSubscriptionResult {
        public required DynamicControllerNamedValue[] dynamicControllerNamedValues { get; set; }

        public class DynamicControllerNamedValue {
            public required string name { get; set; }
            public float value { get; set; }
        }
    }
#pragma warning restore IDE1006 // Naming Styles

    public async Task StartAsync() {
        if (null == _graphQLClient) {
            _logger.Information("[{TAG}] graphql client not set up.", Platform.PlatformName);
            return;
        }

        _logger.Information("[{TAG}] setting up subscription.", Platform.PlatformName);

        _graphQLClient.WebsocketConnectionState.Subscribe(s => {
            _logger.Information("[{TAG}] WebSocketConnectionState:{s}", Platform.PlatformName, s);
        });

        _graphQLClient.WebSocketReceiveErrors.Subscribe(e => {
            // TODO: Do we want to stop app on fail?
            // _applicationLifetime.StopApplication();
            if (e is WebSocketException we)
                _logger.Error("[{TAG}] WebSocketException: {Message} (WebSocketError {WebSocketErrorCode}, ErrorCode {ErrorCode}, NativeErrorCode {NativeErrorCode}",
                    Platform.PlatformName, we.Message, we.WebSocketErrorCode, we.ErrorCode, we.NativeErrorCode);
            else
                _logger.Error("[{TAG}] Exception in websocket receive stream: {e}", Platform.PlatformName, e);
        });

        var controllerValuesRequest = new GraphQLRequest {
            Query = """
subscription ($channelId: String) {
    dynamicControllerNamedValues(
        channelId: $channelId
        ) {
            name
            value
        }
}
""",
            Variables = new {
                channelId = _settings.ChannelId
            }
        };

        IObservable<GraphQLResponse<StreamPadSubscriptionResult>> subscriptionStream =
            _graphQLClient.CreateSubscriptionStream<StreamPadSubscriptionResult>(controllerValuesRequest);

        _subscription = subscriptionStream.Subscribe(
            response => {
                if (null != response.Errors && response.Errors.Length > 0) {
                    // Only the first error really matters
                    _logger.Error("[{TAG}] Graphql exception: {Message}", Platform.PlatformName, response.Errors[0].Message);
                } else if (null != response.Data && null != response.Data.dynamicControllerNamedValues) {
                    _logger.Information("[{TAG}] Received {DynamicControllerNamedValuesLength} controller value(s).",
                        Platform.PlatformName, response.Data.dynamicControllerNamedValues.Length);

                    foreach (StreamPadSubscriptionResult.DynamicControllerNamedValue namedValue in response.Data.dynamicControllerNamedValues) {
                        Task.Run(async () => {
                            if (_rules is StreamPadRules { OnControllerValue: { } callback })
                                await callback(namedValue.name, namedValue.value);
                        });
                    }
                } else {
                    _logger.Error("[{TAG}] Invalid/No response data in subscription", Platform.PlatformName);
                }
            },
            exception => _logger.Error("[{TAG}] Subscription exception: {Exception}", Platform.PlatformName, exception),
            () => _logger.Warning("[{TAG}] Subscription completed", Platform.PlatformName));
        await Task.CompletedTask;
    }

    public async Task StopAsync() {
        _subscription?.Dispose();
        _graphQLClient?.Dispose();
        await Task.CompletedTask;
    }
}
