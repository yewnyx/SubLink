using Serilog;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocket4Net;
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

namespace xyz.yewnyx.SubLink.OBS.OBSClient;

internal sealed class OBSSocketClient(ILogger logger) {
    private const string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";
    private static readonly JsonSerializerOptions _serializationOpt = new() {
        AllowOutOfOrderMetadataProperties = true
    };

    private readonly ILogger _logger = logger;
    private WebSocket? _socket;

    public event EventHandler? OBSConnected;
    public event EventHandler? OBSDisconnected;
    public event EventHandler<OBSErrorArgs>? OBSError;
    public event EventHandler<CurrentSceneCollectionChangingArgs>? CurrentSceneCollectionChanging;
    public event EventHandler<CurrentSceneCollectionChangedArgs>? CurrentSceneCollectionChanged;
    public event EventHandler<SceneCollectionListChangedArgs>? SceneCollectionListChanged;
    public event EventHandler<CurrentProfileChangingArgs>? CurrentProfileChanging;
    public event EventHandler<CurrentProfileChangedArgs>? CurrentProfileChanged;
    public event EventHandler<ProfileListChangedArgs>? ProfileListChanged;
    public event EventHandler<SourceFilterListReindexedArgs>? SourceFilterListReindexed;
    public event EventHandler<SourceFilterCreatedArgs>? SourceFilterCreated;
    public event EventHandler<SourceFilterRemovedArgs>? SourceFilterRemoved;
    public event EventHandler<SourceFilterNameChangedArgs>? SourceFilterNameChanged;
    public event EventHandler<SourceFilterSettingsChangedArgs>? SourceFilterSettingsChanged;
    public event EventHandler<SourceFilterEnableStateChangedArgs>? SourceFilterEnableStateChanged;
    public event EventHandler<ExitStartedArgs>? ExitStarted;
    public event EventHandler<InputCreatedArgs>? InputCreated;
    public event EventHandler<InputRemovedArgs>? InputRemoved;
    public event EventHandler<InputNameChangedArgs>? InputNameChanged;
    public event EventHandler<InputSettingsChangedArgs>? InputSettingsChanged;
    public event EventHandler<InputActiveStateChangedArgs>? InputActiveStateChanged;
    public event EventHandler<InputShowStateChangedArgs>? InputShowStateChanged;
    public event EventHandler<InputMuteStateChangedArgs>? InputMuteStateChanged;
    public event EventHandler<InputVolumeChangedArgs>? InputVolumeChanged;
    public event EventHandler<InputAudioBalanceChangedArgs>? InputAudioBalanceChanged;
    public event EventHandler<InputAudioSyncOffsetChangedArgs>? InputAudioSyncOffsetChanged;
    public event EventHandler<InputAudioTracksChangedArgs>? InputAudioTracksChanged;
    public event EventHandler<InputAudioMonitorTypeChangedArgs>? InputAudioMonitorTypeChanged;
    public event EventHandler<InputVolumeMetersArgs>? InputVolumeMeters;
    public event EventHandler<MediaInputPlaybackStartedArgs>? MediaInputPlaybackStarted;
    public event EventHandler<MediaInputPlaybackEndedArgs>? MediaInputPlaybackEnded;
    public event EventHandler<MediaInputActionTriggeredArgs>? MediaInputActionTriggered;
    public event EventHandler<StreamStateChangedArgs>? StreamStateChanged;
    public event EventHandler<RecordStateChangedArgs>? RecordStateChanged;
    public event EventHandler<RecordFileChangedArgs>? RecordFileChanged;
    public event EventHandler<ReplayBufferStateChangedArgs>? ReplayBufferStateChanged;
    public event EventHandler<VirtualcamStateChangedArgs>? VirtualcamStateChanged;
    public event EventHandler<ReplayBufferSavedArgs>? ReplayBufferSaved;
    public event EventHandler<SceneItemCreatedArgs>? SceneItemCreated;
    public event EventHandler<SceneItemRemovedArgs>? SceneItemRemoved;
    public event EventHandler<SceneItemListReindexedArgs>? SceneItemListReindexed;
    public event EventHandler<SceneItemEnableStateChangedArgs>? SceneItemEnableStateChanged;
    public event EventHandler<SceneItemLockStateChangedArgs>? SceneItemLockStateChanged;
    public event EventHandler<SceneItemSelectedArgs>? SceneItemSelected;
    public event EventHandler<SceneItemTransformChangedArgs>? SceneItemTransformChanged;
    public event EventHandler<SceneCreatedArgs>? SceneCreated;
    public event EventHandler<SceneRemovedArgs>? SceneRemoved;
    public event EventHandler<SceneNameChangedArgs>? SceneNameChanged;
    public event EventHandler<CurrentProgramSceneChangedArgs>? CurrentProgramSceneChanged;
    public event EventHandler<CurrentPreviewSceneChangedArgs>? CurrentPreviewSceneChanged;
    public event EventHandler<SceneListChangedArgs>? SceneListChanged;
    public event EventHandler<CurrentSceneTransitionChangedArgs>? CurrentSceneTransitionChanged;
    public event EventHandler<CurrentSceneTransitionDurationChangedArgs>? CurrentSceneTransitionDurationChanged;
    public event EventHandler<SceneTransitionStartedArgs>? SceneTransitionStarted;
    public event EventHandler<SceneTransitionEndedArgs>? SceneTransitionEnded;
    public event EventHandler<SceneTransitionVideoEndedArgs>? SceneTransitionVideoEnded;
    public event EventHandler<StudioModeStateChangedArgs>? StudioModeStateChanged;
    public event EventHandler<ScreenshotSavedArgs>? ScreenshotSaved;
    public event EventHandler<VendorEventArgs>? VendorEvent;
    public event EventHandler<CustomEventArgs>? CustomEvent;

    private string _serverPassword = string.Empty;
    private uint _rpcVersion = 0;
    private bool _identified = false;
    private readonly Dictionary<string, InResponseMsg.Data?> _requestResults = [];

    public async Task<bool> ConnectAsync(string servcerIp, ushort servcerPort, string servcerPassword) {
        if (_socket != null)
            return true;

        _serverPassword = servcerPassword;

        try {
            _socket = new(
                $"ws://{servcerIp}:{servcerPort}",
                version: WebSocketVersion.Rfc6455,
                userAgent: _userAgent
            ) {
                EnableAutoSendPing = true,
                NoDelay = true
            };

            _socket.Opened += OnSockConnected;
            _socket.Closed += OnSockDisconnected;
            _socket.Error += OnSockError;
            _socket.MessageReceived += OnSockMessageReceived;
            _socket.DataReceived += OnSockDataReceived;

            await _socket.OpenAsync();
        } catch (Exception) {
            return false;
        }

        return true;
    }

    public async Task DisconnectAsync() {
        if (_socket == null)
            return;

        if (_socket.State != WebSocketState.Closed)
            await _socket.CloseAsync();

        _socket = null;
    }

    private void OnSockConnected(object? sender, EventArgs e) =>
        OBSConnected?.Invoke(this, e);

    private void OnSockDisconnected(object? sender, EventArgs e) =>
        OBSDisconnected?.Invoke(this, e);

    private void OnSockError(object? sender, ErrorEventArgs e) =>
        OBSError?.Invoke(this, new(e.Exception));

    private void OnSockMessageReceived(object? sender, MessageReceivedEventArgs e) {
        IBaseMessage? inMsg = JsonSerializer.Deserialize<IBaseMessage>(e.Message, _serializationOpt);
        if (inMsg == null) return;

        switch (inMsg) {
            case InHelloMsg: {
                InHelloMsg helloMsg = (InHelloMsg)inMsg;
                _logger.Information("[{TAG}] Hello received, websocket version {ObsWebSocketVersion}, requested RPC version {RpcVersion}", Platform.PlatformName, helloMsg.D.ObsWebSocketVersion, helloMsg.D.RpcVersion);
                _rpcVersion = helloMsg.D.RpcVersion;
                OutIdentifyMsg ident = new(_rpcVersion);

                if (helloMsg.D.Authentication != null &&
                    !string.IsNullOrEmpty(helloMsg.D.Authentication.Challenge)) {
                    _logger.Information("[{TAG}] Hello indicates authentication requirement, attempting..", Platform.PlatformName);
                    ident.D.Authentication = AuthHashing(
                        helloMsg.D.Authentication.Salt,
                        helloMsg.D.Authentication.Challenge,
                        _serverPassword
                    );
                }

                SendIdentifyMsg(ident);
                break;
            }
            case InIdentifiedMsg: {
                InIdentifiedMsg identMsg = (InIdentifiedMsg)inMsg;
                _logger.Information("[{TAG}] Identified, Negotiated Rpc Version: {NegotiatedRpcVersion}", Platform.PlatformName, identMsg.D.NegotiatedRpcVersion);
                _serverPassword = string.Empty;
                _identified = true;
                break;
            }
            case InEventMsg: {
                InEventMsg eventMsg = (InEventMsg)inMsg;

                switch (eventMsg.D) {
                    case InEventMsg.CurrentSceneCollectionChanging: {
                        CurrentSceneCollectionChanging?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentSceneCollectionChanging)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CurrentSceneCollectionChanged: {
                        CurrentSceneCollectionChanged?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentSceneCollectionChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneCollectionListChanged: {
                        SceneCollectionListChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SceneCollectionListChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CurrentProfileChanging: {
                        CurrentProfileChanging?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentProfileChanging)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CurrentProfileChanged: {
                        CurrentProfileChanged?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentProfileChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.ProfileListChanged: {
                        ProfileListChanged?.Invoke(this, new() {
                            Data = (InEventMsg.ProfileListChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SourceFilterListReindexed: {
                        SourceFilterListReindexed?.Invoke(this, new() {
                            Data = (InEventMsg.SourceFilterListReindexed)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SourceFilterCreated: {
                        SourceFilterCreated?.Invoke(this, new() {
                            Data = (InEventMsg.SourceFilterCreated)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SourceFilterRemoved: {
                        SourceFilterRemoved?.Invoke(this, new() {
                            Data = (InEventMsg.SourceFilterRemoved)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SourceFilterNameChanged: {
                        SourceFilterNameChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SourceFilterNameChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SourceFilterSettingsChanged: {
                        SourceFilterSettingsChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SourceFilterSettingsChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SourceFilterEnableStateChanged: {
                        SourceFilterEnableStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SourceFilterEnableStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.ExitStarted: {
                        ExitStarted?.Invoke(this, new() {
                            Data = (InEventMsg.ExitStarted)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputCreated: {
                        InputCreated?.Invoke(this, new() {
                            Data = (InEventMsg.InputCreated)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputRemoved: {
                        InputRemoved?.Invoke(this, new() {
                            Data = (InEventMsg.InputRemoved)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputNameChanged: {
                        InputNameChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputNameChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputSettingsChanged: {
                        InputSettingsChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputSettingsChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputActiveStateChanged: {
                        InputActiveStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputActiveStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputShowStateChanged: {
                        InputShowStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputShowStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputMuteStateChanged: {
                        InputMuteStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputMuteStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputVolumeChanged: {
                        InputVolumeChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputVolumeChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputAudioBalanceChanged: {
                        InputAudioBalanceChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputAudioBalanceChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputAudioSyncOffsetChanged: {
                        InputAudioSyncOffsetChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputAudioSyncOffsetChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputAudioTracksChanged: {
                        InputAudioTracksChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputAudioTracksChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputAudioMonitorTypeChanged: {
                        InputAudioMonitorTypeChanged?.Invoke(this, new() {
                            Data = (InEventMsg.InputAudioMonitorTypeChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.InputVolumeMeters: {
                        InputVolumeMeters?.Invoke(this, new() {
                            Data = (InEventMsg.InputVolumeMeters)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.MediaInputPlaybackStarted: {
                        MediaInputPlaybackStarted?.Invoke(this, new() {
                            Data = (InEventMsg.MediaInputPlaybackStarted)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.MediaInputPlaybackEnded: {
                        MediaInputPlaybackEnded?.Invoke(this, new() {
                            Data = (InEventMsg.MediaInputPlaybackEnded)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.MediaInputActionTriggered: {
                        MediaInputActionTriggered?.Invoke(this, new() {
                            Data = (InEventMsg.MediaInputActionTriggered)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.StreamStateChanged: {
                        StreamStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.StreamStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.RecordStateChanged: {
                        RecordStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.RecordStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.RecordFileChanged: {
                        RecordFileChanged?.Invoke(this, new() {
                            Data = (InEventMsg.RecordFileChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.ReplayBufferStateChanged: {
                        ReplayBufferStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.ReplayBufferStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.VirtualcamStateChanged: {
                        VirtualcamStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.VirtualcamStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.ReplayBufferSaved: {
                        ReplayBufferSaved?.Invoke(this, new() {
                            Data = (InEventMsg.ReplayBufferSaved)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneItemCreated: {
                        SceneItemCreated?.Invoke(this, new() {
                            Data = (InEventMsg.SceneItemCreated)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneItemRemoved: {
                        SceneItemRemoved?.Invoke(this, new() {
                            Data = (InEventMsg.SceneItemRemoved)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneItemListReindexed: {
                        SceneItemListReindexed?.Invoke(this, new() {
                            Data = (InEventMsg.SceneItemListReindexed)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneItemEnableStateChanged: {
                        SceneItemEnableStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SceneItemEnableStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneItemLockStateChanged: {
                        SceneItemLockStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SceneItemLockStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneItemSelected: {
                        SceneItemSelected?.Invoke(this, new() {
                            Data = (InEventMsg.SceneItemSelected)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneItemTransformChanged: {
                        SceneItemTransformChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SceneItemTransformChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneCreated: {
                        SceneCreated?.Invoke(this, new() {
                            Data = (InEventMsg.SceneCreated)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneRemoved: {
                        SceneRemoved?.Invoke(this, new() {
                            Data = (InEventMsg.SceneRemoved)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneNameChanged: {
                        SceneNameChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SceneNameChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CurrentProgramSceneChanged: {
                        CurrentProgramSceneChanged?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentProgramSceneChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CurrentPreviewSceneChanged: {
                        CurrentPreviewSceneChanged?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentPreviewSceneChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneListChanged: {
                        SceneListChanged?.Invoke(this, new() {
                            Data = (InEventMsg.SceneListChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CurrentSceneTransitionChanged: {
                        CurrentSceneTransitionChanged?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentSceneTransitionChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CurrentSceneTransitionDurationChanged: {
                        CurrentSceneTransitionDurationChanged?.Invoke(this, new() {
                            Data = (InEventMsg.CurrentSceneTransitionDurationChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneTransitionStarted: {
                        SceneTransitionStarted?.Invoke(this, new() {
                            Data = (InEventMsg.SceneTransitionStarted)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneTransitionEnded: {
                        SceneTransitionEnded?.Invoke(this, new() {
                            Data = (InEventMsg.SceneTransitionEnded)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.SceneTransitionVideoEnded: {
                        SceneTransitionVideoEnded?.Invoke(this, new() {
                            Data = (InEventMsg.SceneTransitionVideoEnded)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.StudioModeStateChanged: {
                        StudioModeStateChanged?.Invoke(this, new() {
                            Data = (InEventMsg.StudioModeStateChanged)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.ScreenshotSaved: {
                        ScreenshotSaved?.Invoke(this, new() {
                            Data = (InEventMsg.ScreenshotSaved)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.VendorEvent: {
                        VendorEvent?.Invoke(this, new() {
                            Data = (InEventMsg.VendorEvent)eventMsg.D
                        });
                        break;
                    }
                    case InEventMsg.CustomEvent: {
                        CustomEvent?.Invoke(this, new() {
                            Data = (InEventMsg.CustomEvent)eventMsg.D
                        });
                        break;
                    }
                    default: {
                        _logger.Information(
                            "[{TAG}] Event received, event type: {EventType} , event intent: {EventIntent} , actual type {Type}",
                            Platform.PlatformName,
                            eventMsg.D?.EventType,
                            eventMsg.D?.EventIntent,
                            eventMsg.D?.GetType().ToString() ?? "NULL"
                        );
                        break;
                    }
                }

                break;
            }
            case InResponseMsg: {
                InResponseMsg responseMsg = (InResponseMsg)inMsg;
                _requestResults[responseMsg.D.RequestId] = responseMsg.D;
                break;
            }
            default: {
                _logger.Warning("[{TAG}] Unknown data received, message: {Message}", Platform.PlatformName, e.Message);
                break;
            }
        }
    }

    private void OnSockDataReceived(object? sender, DataReceivedEventArgs e) =>
        _logger.Information("[{TAG}] Data received, length: {Length}", Platform.PlatformName, e.Data.Length);

    private static string AuthHashing(string salt, string challenge, string msg) {
        string hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(msg + salt)));
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(hash + challenge)));
    }

    private void SendIdentifyMsg(OutIdentifyMsg msg) =>
        _socket?.Send(JsonSerializer.Serialize(msg));

    public async Task<InResponseMsg.Data?> SendDataAsync(OutRequestMsg msg) {
        while (!_identified) {
            await Task.Delay(10);
        }

        if (msg.D.RequestData != null)
            msg.D.RequestType = msg.D.RequestData.GetType().Name;

        _socket?.Send(JsonSerializer.Serialize(msg));
        _requestResults[msg.D.RequestId] = null;
        InResponseMsg.Data? result;

        do {
            await Task.Delay(10); // Add a little delay to avoid hogging resources
            result = _requestResults[msg.D.RequestId];
        } while (result == null);

        return result;
    }
}
