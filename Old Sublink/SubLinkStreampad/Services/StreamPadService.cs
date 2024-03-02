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

namespace xyz.yewnyx.SubLink;

[UsedImplicitly]
internal sealed partial class StreamPadService : IService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<StreamPadSettings> _settingsMonitor;
    private StreamPadSettings _settings;

    private readonly IStreamPadRules _rules;

    private IDisposable _subscription;
    private GraphQLHttpClient _graphQLClient;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _streamPadLoggedInScope;

    public StreamPadService(
        ILogger logger,
        StreampadGlobals globals,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<StreamPadSettings> settingsMonitor,
        IServiceProvider serviceProvider,
        IStreamPadRules rules)
    {
        _logger = logger;
        globals.serviceProvider = serviceProvider;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateStreampadSettings);
        _settings = _settingsMonitor.CurrentValue;

        _rules = rules;

        if (!string.IsNullOrWhiteSpace(_settings.WebSocketUrl) && !string.IsNullOrWhiteSpace(_settings.ChannelId)) {
            _logger.Information("StreamPad | setting up graphql client.");
            _graphQLClient = new GraphQLHttpClient(o => {
                o.WebSocketEndPoint = new Uri(_settings.WebSocketUrl);
                o.WebSocketProtocol = "graphql-ws";
            }, new NewtonsoftJsonSerializer());
        } else {
            _logger.Information("StreamPad | settings misconfigured.");
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

    public async Task Start() {
        if (null == _graphQLClient) {
            _logger.Information("StreamPad | graphql client not set up.");
            return;
        }

        _logger.Information("StreamPad | setting up subscription.");

        _graphQLClient.WebsocketConnectionState.Subscribe(s => {
            _logger.Information($"StreamPad | WebSocketConnectionState:{s}");
        });

        _graphQLClient.WebSocketReceiveErrors.Subscribe(e => {
            // TODO: Do we want to stop app on fail?
            // _applicationLifetime.StopApplication();
            if (e is WebSocketException we)
                _logger.Error($"StreamPad | WebSocketException: {we.Message} (WebSocketError {we.WebSocketErrorCode}, ErrorCode {we.ErrorCode}, NativeErrorCode {we.NativeErrorCode}");
            else
                _logger.Error($"StreamPad | Exception in websocket receive stream: {e.ToString()}");
        });

        var controllerValuesRequest = new GraphQLRequest {
            Query = @"
            subscription ($channelId: String) {
                dynamicControllerNamedValues(
                    channelId: $channelId
                    ) {
                        name
                        value
                    }
            }",
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
                    _logger.Error($"StreamPad | Graphql exception: {response.Errors[0].Message}");
                } else if (null != response.Data && null != response.Data.dynamicControllerNamedValues) {
                    _logger.Information($"StreamPad | Received {response.Data.dynamicControllerNamedValues.Length} controller value(s).");

                    foreach (StreamPadSubscriptionResult.DynamicControllerNamedValue namedValue in response.Data.dynamicControllerNamedValues) {
                        Task.Run(async () => {
                            if (_rules is StreamPadRules { OnControllerValue: { } callback })
                                await callback(namedValue.name, namedValue.value);
                        });
                    }
                } else {
                    _logger.Error($"StreamPad | Invalid/No response data in subscription");
                }
            },
            exception => _logger.Error($"StreamPad | Subscription exception: {exception}"),
            () => _logger.Warning($"StreamPad | Subscription completed"));

        await Task.CompletedTask;
    }

    public async Task Stop() {
        _subscription?.Dispose();
        _graphQLClient?.Dispose();
        await Task.CompletedTask;
    }
}
