using System.Threading.Tasks;
using TwitchLib.Client.Events;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Stream;

namespace xyz.yewnyx.SubLink.Twitch.Services;

internal sealed partial class TwitchService {
    private void WireCallbacks() {
        _eventSub.ChannelCheer += OnChannelCheer;
        _eventSub.ChannelFollow += OnChannelFollow;
        _eventSub.ChannelHypeTrainBegin += OnChannelHypeTrainBegin;
        _eventSub.ChannelHypeTrainEnd += OnChannelHypeTrainEnd;
        _eventSub.ChannelHypeTrainProgress += OnChannelHypeTrainProgress;
        _eventSub.ChannelPointsCustomRewardRedemptionAdd += OnChannelPointsCustomRewardRedemptionAdd;
        _eventSub.ChannelPointsCustomRewardRedemptionUpdate += OnChannelPointsCustomRewardRedemptionUpdate;
        _eventSub.ChannelPollBegin += OnChannelPollBegin;
        _eventSub.ChannelPollEnd += OnChannelPollEnd;
        _eventSub.ChannelPollProgress += OnChannelPollProgress;
        _eventSub.ChannelPredictionBegin += OnChannelPredictionBegin;
        _eventSub.ChannelPredictionEnd += OnChannelPredictionEnd;
        _eventSub.ChannelPredictionLock += OnChannelPredictionLock;
        _eventSub.ChannelPredictionProgress += OnChannelPredictionProgress;
        _eventSub.ChannelRaid += OnChannelRaid;
        _eventSub.ChannelSubscribe += OnChannelSubscribe;
        _eventSub.ChannelSubscriptionEnd += OnChannelSubscriptionEnd;
        _eventSub.ChannelSubscriptionGift += OnChannelSubscriptionGift;
        _eventSub.ChannelSubscriptionMessage += OnChannelSubscriptionMessage;
        _eventSub.ChannelUpdate += OnChannelUpdate;
        _eventSub.ErrorOccurred += OnErrorOccurred;
        _eventSub.StreamOffline += OnStreamOffline;
        _eventSub.StreamOnline += OnStreamOnline;
        _eventSub.WebsocketDisconnected += OnWebsocketDisconnected;
        _eventSub.WebsocketReconnected += OnWebsocketReconnected;
        _eventSub.WebsocketConnected += OnWebsocketConnected;
    }

    private void OnJoinedChannel(object? sender, OnJoinedChannelArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnJoinedChannel: { } callback})
                await callback(e.Channel, e.BotUsername);
        });
    }

    private void OnMessageReceived(object? sender, OnMessageReceivedArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnMessageReceived: { } callback})
                await callback(e.ChatMessage);
        });
    }
    
    private void OnChannelCheer(object? sender, ChannelCheerArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnCheer: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelFollow(object? sender, ChannelFollowArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnFollow: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelHypeTrainBegin(object? sender, ChannelHypeTrainBeginArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnHypeTrainBegin: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelHypeTrainEnd(object? sender, ChannelHypeTrainEndArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnHypeTrainEnd: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelHypeTrainProgress(object? sender, ChannelHypeTrainProgressArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnHypeTrainProgress: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelPointsCustomRewardRedemptionAdd(object? sender, ChannelPointsCustomRewardRedemptionArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPointsCustomRewardRedemptionAdd: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void
        OnChannelPointsCustomRewardRedemptionUpdate(object? sender, ChannelPointsCustomRewardRedemptionArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPointsCustomRewardRedemptionUpdate: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelPollBegin(object? sender, ChannelPollBeginArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPollBegin: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelPollEnd(object? sender, ChannelPollEndArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPollEnd: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelPollProgress(object? sender, ChannelPollProgressArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPollProgress: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelPredictionBegin(object? sender, ChannelPredictionBeginArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPredictionBegin: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelPredictionEnd(object? sender, ChannelPredictionEndArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPredictionEnd: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelPredictionLock(object? sender, ChannelPredictionLockArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPredictionLock: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelPredictionProgress(object? sender, ChannelPredictionProgressArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnPredictionProgress: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelRaid(object? sender, ChannelRaidArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnRaid: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelSubscribe(object? sender, ChannelSubscribeArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnSubscribe: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelSubscriptionEnd(object? sender, ChannelSubscriptionEndArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnSubscriptionEnd: { } callback})
                await callback(e.Notification.Payload.Event);

            _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        });
    }

    private void OnChannelSubscriptionGift(object? sender, ChannelSubscriptionGiftArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnSubscriptionGift: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelSubscriptionMessage(object? sender, ChannelSubscriptionMessageArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnSubscriptionMessage: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnChannelUpdate(object? sender, ChannelUpdateArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnChannelUpdate: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnErrorOccurred(object? sender, ErrorOccuredArgs e) {
        _logger.Error(e.Exception, "[{TAG}] Websocket {EventSubSessionId}: {EMessage}",
            Platform.PlatformName, _eventSub.SessionId, e.Message);
    }

    private void OnStreamOffline(object? sender, StreamOfflineArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnStreamOffline: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }

    private void OnStreamOnline(object? sender, StreamOnlineArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules {OnStreamOnline: { } callback})
                await callback(e.Notification.Payload.Event);
        });
    }
}
