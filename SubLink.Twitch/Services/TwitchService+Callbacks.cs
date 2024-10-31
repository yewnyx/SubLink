using System.Threading.Tasks;
using TwitchLib.Client.Events;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Stream;

namespace xyz.yewnyx.SubLink.Twitch.Services;

internal sealed partial class TwitchService {
    private void WireCallbacks() {
        _eventSub.WebsocketConnected += OnWebsocketConnected;
        _eventSub.WebsocketDisconnected += OnWebsocketDisconnected;
        _eventSub.WebsocketReconnected += OnWebsocketReconnected;
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
    
    private async Task OnChannelCheer(object? sender, ChannelCheerArgs e) {
        if (_rules is TwitchRules {OnCheer: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelFollow(object? sender, ChannelFollowArgs e) {
        if (_rules is TwitchRules {OnFollow: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelHypeTrainBegin(object? sender, ChannelHypeTrainBeginArgs e) {
        if (_rules is TwitchRules {OnHypeTrainBegin: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelHypeTrainEnd(object? sender, ChannelHypeTrainEndArgs e) {
        if (_rules is TwitchRules {OnHypeTrainEnd: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelHypeTrainProgress(object? sender, ChannelHypeTrainProgressArgs e) {
        if (_rules is TwitchRules {OnHypeTrainProgress: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelPointsCustomRewardRedemptionAdd(object? sender, ChannelPointsCustomRewardRedemptionArgs e) {
        if (_rules is TwitchRules {OnPointsCustomRewardRedemptionAdd: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelPointsCustomRewardRedemptionUpdate(object? sender, ChannelPointsCustomRewardRedemptionArgs e) {
        if (_rules is TwitchRules {OnPointsCustomRewardRedemptionUpdate: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPollBegin(object? sender, ChannelPollBeginArgs e) {
        if (_rules is TwitchRules {OnPollBegin: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPollEnd(object? sender, ChannelPollEndArgs e) {
        if (_rules is TwitchRules {OnPollEnd: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPollProgress(object? sender, ChannelPollProgressArgs e) {
        if (_rules is TwitchRules {OnPollProgress: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionBegin(object? sender, ChannelPredictionBeginArgs e) {
        if (_rules is TwitchRules {OnPredictionBegin: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionEnd(object? sender, ChannelPredictionEndArgs e) {
        if (_rules is TwitchRules {OnPredictionEnd: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionLock(object? sender, ChannelPredictionLockArgs e) {
        if (_rules is TwitchRules {OnPredictionLock: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionProgress(object? sender, ChannelPredictionProgressArgs e) {
        if (_rules is TwitchRules {OnPredictionProgress: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelRaid(object? sender, ChannelRaidArgs e) {
        if (_rules is TwitchRules {OnRaid: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelSubscribe(object? sender, ChannelSubscribeArgs e) {
        if (_rules is TwitchRules {OnSubscribe: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelSubscriptionEnd(object? sender, ChannelSubscriptionEndArgs e) {
        if (_rules is TwitchRules {OnSubscriptionEnd: { } callback})
            await callback(e.Notification.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelSubscriptionGift(object? sender, ChannelSubscriptionGiftArgs e) {
        if (_rules is TwitchRules {OnSubscriptionGift: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelSubscriptionMessage(object? sender, ChannelSubscriptionMessageArgs e) {
        if (_rules is TwitchRules {OnSubscriptionMessage: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnChannelUpdate(object? sender, ChannelUpdateArgs e) {
        if (_rules is TwitchRules {OnChannelUpdate: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnErrorOccurred(object? sender, ErrorOccuredArgs e) {
        _logger.Error(e.Exception, "[{TAG}] Websocket {EventSubSessionId}: {EMessage}",
            Platform.PlatformName, _eventSub.SessionId, e.Message);
        await Task.CompletedTask;
    }

    private async Task OnStreamOffline(object? sender, StreamOfflineArgs e) {
        if (_rules is TwitchRules {OnStreamOffline: { } callback})
            await callback(e.Notification.Payload.Event);
    }

    private async Task OnStreamOnline(object? sender, StreamOnlineArgs e) {
        if (_rules is TwitchRules {OnStreamOnline: { } callback})
            await callback(e.Notification.Payload.Event);
    }
}
