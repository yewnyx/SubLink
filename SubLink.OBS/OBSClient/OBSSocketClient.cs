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
using xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Event;

namespace xyz.yewnyx.SubLink.OBS.OBSClient;

internal sealed class OBSSocketClient(ILogger logger) {
    private const string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";
    private static readonly JsonSerializerOptions _serializationOpt = new() {
        AllowOutOfOrderMetadataProperties = true
    };

    private readonly ILogger _logger = logger;
    private WebSocket? _socket;

    public event EventHandler? OnOBSConnected;
    public event EventHandler? OnOBSDisconnected;
    public event EventHandler<OBSErrorArgs>? OnOBSError;
    public event EventHandler<CurrentSceneCollectionChangingArgs>? OnCurrentSceneCollectionChanging;
    public event EventHandler<CurrentSceneCollectionChangedArgs>? OnCurrentSceneCollectionChanged;
    public event EventHandler<SceneCollectionListChangedArgs>? OnSceneCollectionListChanged;
    public event EventHandler<CurrentProfileChangingArgs>? OnCurrentProfileChanging;
    public event EventHandler<CurrentProfileChangedArgs>? OnCurrentProfileChanged;
    public event EventHandler<ProfileListChangedArgs>? OnProfileListChanged;
    public event EventHandler<SourceFilterListReindexedArgs>? OnSourceFilterListReindexed;
    public event EventHandler<SourceFilterCreatedArgs>? OnSourceFilterCreated;
    public event EventHandler<SourceFilterRemovedArgs>? OnSourceFilterRemoved;
    public event EventHandler<SourceFilterNameChangedArgs>? OnSourceFilterNameChanged;
    public event EventHandler<SourceFilterSettingsChangedArgs>? OnSourceFilterSettingsChanged;
    public event EventHandler<SourceFilterEnableStateChangedArgs>? OnSourceFilterEnableStateChanged;
    public event EventHandler? OnExitStarted;
    public event EventHandler<InputCreatedArgs>? OnInputCreated;
    public event EventHandler<InputRemovedArgs>? OnInputRemoved;
    public event EventHandler<InputNameChangedArgs>? OnInputNameChanged;
    public event EventHandler<InputSettingsChangedArgs>? OnInputSettingsChanged;
    public event EventHandler<InputActiveStateChangedArgs>? OnInputActiveStateChanged;
    public event EventHandler<InputShowStateChangedArgs>? OnInputShowStateChanged;
    public event EventHandler<InputMuteStateChangedArgs>? OnInputMuteStateChanged;
    public event EventHandler<InputVolumeChangedArgs>? OnInputVolumeChanged;
    public event EventHandler<InputAudioBalanceChangedArgs>? OnInputAudioBalanceChanged;
    public event EventHandler<InputAudioSyncOffsetChangedArgs>? OnInputAudioSyncOffsetChanged;
    public event EventHandler<InputAudioTracksChangedArgs>? OnInputAudioTracksChanged;
    public event EventHandler<InputAudioMonitorTypeChangedArgs>? OnInputAudioMonitorTypeChanged;
    public event EventHandler<InputVolumeMetersArgs>? OnInputVolumeMeters;
    public event EventHandler<MediaInputPlaybackStartedArgs>? OnMediaInputPlaybackStarted;
    public event EventHandler<MediaInputPlaybackEndedArgs>? OnMediaInputPlaybackEnded;
    public event EventHandler<MediaInputActionTriggeredArgs>? OnMediaInputActionTriggered;
    public event EventHandler<StreamStateChangedArgs>? OnStreamStateChanged;
    public event EventHandler<RecordStateChangedArgs>? OnRecordStateChanged;
    public event EventHandler<RecordFileChangedArgs>? OnRecordFileChanged;
    public event EventHandler<ReplayBufferStateChangedArgs>? OnReplayBufferStateChanged;
    public event EventHandler<VirtualcamStateChangedArgs>? OnVirtualcamStateChanged;
    public event EventHandler<ReplayBufferSavedArgs>? OnReplayBufferSaved;
    public event EventHandler<SceneItemCreatedArgs>? OnSceneItemCreated;
    public event EventHandler<SceneItemRemovedArgs>? OnSceneItemRemoved;
    public event EventHandler<SceneItemListReindexedArgs>? OnSceneItemListReindexed;
    public event EventHandler<SceneItemEnableStateChangedArgs>? OnSceneItemEnableStateChanged;
    public event EventHandler<SceneItemLockStateChangedArgs>? OnSceneItemLockStateChanged;
    public event EventHandler<SceneItemSelectedArgs>? OnSceneItemSelected;
    public event EventHandler<SceneItemTransformChangedArgs>? OnSceneItemTransformChanged;
    public event EventHandler<SceneCreatedArgs>? OnSceneCreated;
    public event EventHandler<SceneRemovedArgs>? OnSceneRemoved;
    public event EventHandler<SceneNameChangedArgs>? OnSceneNameChanged;
    public event EventHandler<CurrentProgramSceneChangedArgs>? OnCurrentProgramSceneChanged;
    public event EventHandler<CurrentPreviewSceneChangedArgs>? OnCurrentPreviewSceneChanged;
    public event EventHandler<SceneListChangedArgs>? OnSceneListChanged;
    public event EventHandler<CurrentSceneTransitionChangedArgs>? OnCurrentSceneTransitionChanged;
    public event EventHandler<CurrentSceneTransitionDurationChangedArgs>? OnCurrentSceneTransitionDurationChanged;
    public event EventHandler<SceneTransitionStartedArgs>? OnSceneTransitionStarted;
    public event EventHandler<SceneTransitionEndedArgs>? OnSceneTransitionEnded;
    public event EventHandler<SceneTransitionVideoEndedArgs>? OnSceneTransitionVideoEnded;
    public event EventHandler<StudioModeStateChangedArgs>? OnStudioModeStateChanged;
    public event EventHandler<ScreenshotSavedArgs>? OnScreenshotSaved;
    public event EventHandler<VendorEventArgs>? OnVendorEvent;
    public event EventHandler<CustomEventArgs>? OnCustomEvent;

    public bool Enabled { get; internal set; } = false;
    private string _serverPassword = string.Empty;
    private uint _rpcVersion = 0;
    private bool _identified = false;
    private readonly Dictionary<string, InResponseMsg.Data?> _requestResults = [];

    public async Task<bool> ConnectAsync(string servcerIp, ushort servcerPort, string servcerPassword) {
        if (_socket != null) return true;
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
        if (_socket == null) return;
        if (_socket.State != WebSocketState.Closed)
            await _socket.CloseAsync();

        _socket = null;
    }

    private void OnSockConnected(object? sender, EventArgs e) =>
        OnOBSConnected?.Invoke(this, e);

    private void OnSockDisconnected(object? sender, EventArgs e) =>
        OnOBSDisconnected?.Invoke(this, e);

    private void OnSockError(object? sender, ErrorEventArgs e) =>
        OnOBSError?.Invoke(this, new(e.Exception));

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
                    case CurrentSceneCollectionChanging: {
                        OnCurrentSceneCollectionChanging?.Invoke(this, new() {
                            Data = ((CurrentSceneCollectionChanging)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CurrentSceneCollectionChanged: {
                        OnCurrentSceneCollectionChanged?.Invoke(this, new() {
                            Data = ((CurrentSceneCollectionChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneCollectionListChanged: {
                        OnSceneCollectionListChanged?.Invoke(this, new() {
                            Data = ((SceneCollectionListChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CurrentProfileChanging: {
                        OnCurrentProfileChanging?.Invoke(this, new() {
                            Data = ((CurrentProfileChanging)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CurrentProfileChanged: {
                        OnCurrentProfileChanged?.Invoke(this, new() {
                            Data = ((CurrentProfileChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case ProfileListChanged: {
                        OnProfileListChanged?.Invoke(this, new() {
                            Data = ((ProfileListChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SourceFilterListReindexed: {
                        OnSourceFilterListReindexed?.Invoke(this, new() {
                            Data = ((SourceFilterListReindexed)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SourceFilterCreated: {
                        OnSourceFilterCreated?.Invoke(this, new() {
                            Data = ((SourceFilterCreated)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SourceFilterRemoved: {
                        OnSourceFilterRemoved?.Invoke(this, new() {
                            Data = ((SourceFilterRemoved)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SourceFilterNameChanged: {
                        OnSourceFilterNameChanged?.Invoke(this, new() {
                            Data = ((SourceFilterNameChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SourceFilterSettingsChanged: {
                        OnSourceFilterSettingsChanged?.Invoke(this, new() {
                            Data = ((SourceFilterSettingsChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SourceFilterEnableStateChanged: {
                        OnSourceFilterEnableStateChanged?.Invoke(this, new() {
                            Data = ((SourceFilterEnableStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case ExitStarted: {
                        OnExitStarted?.Invoke(this, new());
                        break;
                    }
                    case InputCreated: {
                        OnInputCreated?.Invoke(this, new() {
                            Data = ((InputCreated)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputRemoved: {
                        OnInputRemoved?.Invoke(this, new() {
                            Data = ((InputRemoved)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputNameChanged: {
                        OnInputNameChanged?.Invoke(this, new() {
                            Data = ((InputNameChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputSettingsChanged: {
                        OnInputSettingsChanged?.Invoke(this, new() {
                            Data = ((InputSettingsChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputActiveStateChanged: {
                        OnInputActiveStateChanged?.Invoke(this, new() {
                            Data = ((InputActiveStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputShowStateChanged: {
                        OnInputShowStateChanged?.Invoke(this, new() {
                            Data = ((InputShowStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputMuteStateChanged: {
                        OnInputMuteStateChanged?.Invoke(this, new() {
                            Data = ((InputMuteStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputVolumeChanged: {
                        OnInputVolumeChanged?.Invoke(this, new() {
                            Data = ((InputVolumeChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputAudioBalanceChanged: {
                        OnInputAudioBalanceChanged?.Invoke(this, new() {
                            Data = ((InputAudioBalanceChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputAudioSyncOffsetChanged: {
                        OnInputAudioSyncOffsetChanged?.Invoke(this, new() {
                            Data = ((InputAudioSyncOffsetChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputAudioTracksChanged: {
                        OnInputAudioTracksChanged?.Invoke(this, new() {
                            Data = ((InputAudioTracksChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputAudioMonitorTypeChanged: {
                        OnInputAudioMonitorTypeChanged?.Invoke(this, new() {
                            Data = ((InputAudioMonitorTypeChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case InputVolumeMeters: {
                        OnInputVolumeMeters?.Invoke(this, new() {
                            Data = ((InputVolumeMeters)eventMsg.D).EventData
                        });
                        break;
                    }
                    case MediaInputPlaybackStarted: {
                        OnMediaInputPlaybackStarted?.Invoke(this, new() {
                            Data = ((MediaInputPlaybackStarted)eventMsg.D).EventData
                        });
                        break;
                    }
                    case MediaInputPlaybackEnded: {
                        OnMediaInputPlaybackEnded?.Invoke(this, new() {
                            Data = ((MediaInputPlaybackEnded)eventMsg.D).EventData
                        });
                        break;
                    }
                    case MediaInputActionTriggered: {
                        OnMediaInputActionTriggered?.Invoke(this, new() {
                            Data = ((MediaInputActionTriggered)eventMsg.D).EventData
                        });
                        break;
                    }
                    case StreamStateChanged: {
                        OnStreamStateChanged?.Invoke(this, new() {
                            Data = ((StreamStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case RecordStateChanged: {
                        OnRecordStateChanged?.Invoke(this, new() {
                            Data = ((RecordStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case RecordFileChanged: {
                        OnRecordFileChanged?.Invoke(this, new() {
                            Data = ((RecordFileChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case ReplayBufferStateChanged: {
                        OnReplayBufferStateChanged?.Invoke(this, new() {
                            Data = ((ReplayBufferStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case VirtualcamStateChanged: {
                        OnVirtualcamStateChanged?.Invoke(this, new() {
                            Data = ((VirtualcamStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case ReplayBufferSaved: {
                        OnReplayBufferSaved?.Invoke(this, new() {
                            Data = ((ReplayBufferSaved)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneItemCreated: {
                        OnSceneItemCreated?.Invoke(this, new() {
                            Data = ((SceneItemCreated)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneItemRemoved: {
                        OnSceneItemRemoved?.Invoke(this, new() {
                            Data = ((SceneItemRemoved)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneItemListReindexed: {
                        OnSceneItemListReindexed?.Invoke(this, new() {
                            Data = ((SceneItemListReindexed)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneItemEnableStateChanged: {
                        OnSceneItemEnableStateChanged?.Invoke(this, new() {
                            Data = ((SceneItemEnableStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneItemLockStateChanged: {
                        OnSceneItemLockStateChanged?.Invoke(this, new() {
                            Data = ((SceneItemLockStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneItemSelected: {
                        OnSceneItemSelected?.Invoke(this, new() {
                            Data = ((SceneItemSelected)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneItemTransformChanged: {
                        OnSceneItemTransformChanged?.Invoke(this, new() {
                            Data = ((SceneItemTransformChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneCreated: {
                        OnSceneCreated?.Invoke(this, new() {
                            Data = ((SceneCreated)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneRemoved: {
                        OnSceneRemoved?.Invoke(this, new() {
                            Data = ((SceneRemoved)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneNameChanged: {
                        OnSceneNameChanged?.Invoke(this, new() {
                            Data = ((SceneNameChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CurrentProgramSceneChanged: {
                        OnCurrentProgramSceneChanged?.Invoke(this, new() {
                            Data = ((CurrentProgramSceneChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CurrentPreviewSceneChanged: {
                        OnCurrentPreviewSceneChanged?.Invoke(this, new() {
                            Data = ((CurrentPreviewSceneChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneListChanged: {
                        OnSceneListChanged?.Invoke(this, new() {
                            Data = ((SceneListChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CurrentSceneTransitionChanged: {
                        OnCurrentSceneTransitionChanged?.Invoke(this, new() {
                            Data = ((CurrentSceneTransitionChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CurrentSceneTransitionDurationChanged: {
                        OnCurrentSceneTransitionDurationChanged?.Invoke(this, new() {
                            Data = ((CurrentSceneTransitionDurationChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneTransitionStarted: {
                        OnSceneTransitionStarted?.Invoke(this, new() {
                            Data = ((SceneTransitionStarted)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneTransitionEnded: {
                        OnSceneTransitionEnded?.Invoke(this, new() {
                            Data = ((SceneTransitionEnded)eventMsg.D).EventData
                        });
                        break;
                    }
                    case SceneTransitionVideoEnded: {
                        OnSceneTransitionVideoEnded?.Invoke(this, new() {
                            Data = ((SceneTransitionVideoEnded)eventMsg.D).EventData
                        });
                        break;
                    }
                    case StudioModeStateChanged: {
                        OnStudioModeStateChanged?.Invoke(this, new() {
                            Data = ((StudioModeStateChanged)eventMsg.D).EventData
                        });
                        break;
                    }
                    case ScreenshotSaved: {
                        OnScreenshotSaved?.Invoke(this, new() {
                            Data = ((ScreenshotSaved)eventMsg.D).EventData
                        });
                        break;
                    }
                    case VendorEvent: {
                        OnVendorEvent?.Invoke(this, new() {
                            Data = ((VendorEvent)eventMsg.D).EventData
                        });
                        break;
                    }
                    case CustomEvent: {
                        OnCustomEvent?.Invoke(this, new() {
                            Data = ((CustomEvent)eventMsg.D).EventData
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
        if (!Enabled) return null;

        while (!_identified) {
            await Task.Delay(10);
        }

        if (msg.D.RequestData != null)
            msg.D.RequestType = msg.D.RequestData.GetType().Name;

        _requestResults[msg.D.RequestId] = null;
        InResponseMsg.Data? result;
        _socket?.Send(JsonSerializer.Serialize(msg));

        do {
            await Task.Delay(10); // Add a little delay to avoid hogging resources
            result = _requestResults[msg.D.RequestId];
        } while (result == null);

        // We're too fast for OBS's websocket.. =I
        // introduce a little delay to avoid OBS completely messing up requests
        await Task.Delay(50);
        return result;
    }
}
