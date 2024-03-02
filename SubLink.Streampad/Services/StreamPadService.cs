using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace xyz.yewnyx.SubLink.Streampad.Services;

[UsedImplicitly]
internal sealed class StreamPadService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<StreamPadSettings> _settingsMonitor;
    private StreamPadSettings _settings;

    private readonly StreamPadRules _rules;

    private IDisposable _subscription;
    private GraphQLHttpClient _graphQLClient;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _streamPadLoggedInScope;

    public StreamPadService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<StreamPadSettings> settingsMonitor,
        StreamPadRules rules) {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateStreampadSettings);
        _settings = _settingsMonitor.CurrentValue;

        _rules = rules;

        if (!string.IsNullOrWhiteSpace(_settings.WebSocketUrl) && !string.IsNullOrWhiteSpace(_settings.ChannelId)) {
            _logger.Information("[{TAG}] setting up graphql client.", "StreamPad");
            _graphQLClient = new GraphQLHttpClient(o => {
                o.WebSocketEndPoint = new Uri(_settings.WebSocketUrl);
                o.WebSocketProtocol = "graphql-ws";
            }, new NewtonsoftJsonSerializer());
        } else {
            _logger.Information("[{TAG}] settings misconfigured.", "StreamPad");
        }
    }

    private void UpdateStreampadSettings(StreamPadSettings settings) => _settings = settings;

    public class StreamPadSubscriptionResult {
        public DynamicControllerNamedValue[] dynamicControllerNamedValues { get; set; }

        public class DynamicControllerNamedValue {
            public string name { get; set; }
            public float value { get; set; }
        }
    }

    public async Task StartAsync() {
        if (null == _graphQLClient) {
            _logger.Information("[{TAG}] graphql client not set up.", "StreamPad");
            return;
        }

        _logger.Information("[{TAG}] setting up subscription.", "StreamPad");

        _graphQLClient.WebsocketConnectionState.Subscribe(s => {
            _logger.Information("[{TAG}] WebSocketConnectionState:{s}", "StreamPad", s);
        });

        _graphQLClient.WebSocketReceiveErrors.Subscribe(e => {
            // TODO: Do we want to stop app on fail?
            // _applicationLifetime.StopApplication();
            if (e is WebSocketException we)
                _logger.Error("[{TAG}] WebSocketException: {Message} (WebSocketError {WebSocketErrorCode}, ErrorCode {ErrorCode}, NativeErrorCode {NativeErrorCode}",
                    "StreamPad", we.Message, we.WebSocketErrorCode, we.ErrorCode, we.NativeErrorCode);
            else
                _logger.Error("[{TAG}] Exception in websocket receive stream: {e}", "StreamPad", e);
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
                    _logger.Error("[{TAG}] Graphql exception: {Message}", "StreamPad", response.Errors[0].Message);
                } else if (null != response.Data && null != response.Data.dynamicControllerNamedValues) {
                    _logger.Information("[{TAG}] Received {DynamicControllerNamedValuesLength} controller value(s).",
                        "StreamPad", response.Data.dynamicControllerNamedValues.Length);

                    foreach (StreamPadSubscriptionResult.DynamicControllerNamedValue namedValue in response.Data.dynamicControllerNamedValues) {
                        Task.Run(async () => {
                            if (_rules is StreamPadRules { OnControllerValue: { } callback })
                                await callback(namedValue.name, namedValue.value);
                        });
                    }
                } else {
                    _logger.Error("[{TAG}] Invalid/No response data in subscription", "StreamPad");
                }
            },
            exception => _logger.Error("[{TAG}] Subscription exception: {Exception}", "StreamPad", exception),
            () => _logger.Warning("[{TAG}] Subscription completed", "StreamPad"));
    }

    public async Task StopAsync() {
        _subscription?.Dispose();
        _graphQLClient?.Dispose();
        await Task.CompletedTask;
    }
}
