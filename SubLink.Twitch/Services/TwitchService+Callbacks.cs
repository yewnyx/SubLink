using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Models;
using TwitchLib.EventSub.Core.EventArgs.Channel;
using TwitchLib.EventSub.Core.EventArgs.Stream;
using TwitchLib.EventSub.Websockets.Core.EventArgs;

namespace xyz.yewnyx.SubLink.Twitch.Services;

internal sealed partial class TwitchService {
    private void WireCallbacks() {
        _eventSub.WebsocketConnected += OnWebsocketConnected;
        _eventSub.WebsocketDisconnected += OnWebsocketDisconnected;
        _eventSub.WebsocketReconnected += OnWebsocketReconnected;
        _eventSub.ChannelBitsUse += OnChannelBitsUse;
        _eventSub.ChannelChatMessage += OnChannelChatMessage;
        _eventSub.ChannelCheer += OnChannelCheer;
        _eventSub.ChannelFollow += OnChannelFollow;
        _eventSub.ChannelHypeTrainBeginV2 += OnChannelHypeTrainBegin;
        _eventSub.ChannelHypeTrainEndV2 += OnChannelHypeTrainEnd;
        _eventSub.ChannelHypeTrainProgressV2 += OnChannelHypeTrainProgress;
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

    /*
    private void OnJoinedChannel(object? sender, OnJoinedChannelArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules { OnJoinedChannel: { } callback })
                await callback(e.Channel, e.BotUsername);
        });
    }

    private void OnMessageReceived(object? sender, OnMessageReceivedArgs e) {
        Task.Run(async () => {
            if (_rules is TwitchRules { OnMessageReceived: { } callback })
                await callback(e.ChatMessage);
        });
    }
    */

    private async Task OnChannelBitsUse(object? sender, ChannelBitsUseArgs e) {
        if (_rules is TwitchRules { OnChannelBitsUse: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelChatMessage(object? sender, ChannelChatMessageArgs e) {
        var modernMsg = e.Payload.Event;

        if (_rules is TwitchRules { OnChatMessage: { } callback })
            await callback(modernMsg);

        UserType modernMsgToUserType() {
            if (modernMsg.IsBroadcaster)
                return UserType.Broadcaster;
            if (modernMsg.IsModerator)
                return UserType.Moderator;
            if (modernMsg.IsStaff)
                return UserType.Staff;
            return UserType.Viewer;
        }

        int bits = modernMsg.Cheer?.Bits ?? 0;

        double ConvertBitsToUsd() {
            
            if (bits < 1500)
                return (double)bits / 100.0 * 1.4;

            if (bits < 5000)
                return (double)bits / 1500.0 * 19.95;

            if (bits < 10000)
                return (double)bits / 5000.0 * 64.4;

            if (bits < 25000)
                return (double)bits / 10000.0 * 126.0;

            return (double)bits / 25000.0 * 308.0;
        }

        var subBadge = modernMsg.Badges.FirstOrDefault(x => x.SetId.Equals("subscriber", StringComparison.InvariantCultureIgnoreCase));
        var subBadgeInfo = subBadge?.Info ?? "0";
        var cheerBadge = modernMsg.Badges.FirstOrDefault(x => x.SetId.Contains("cheer", StringComparison.InvariantCultureIgnoreCase));
        var cheerBadgeInfo = cheerBadge?.Info ?? "0";
        var badges = modernMsg.Badges.Select(x => new KeyValuePair<string, string>(x.SetId, x.Info)).ToList();

        var udFlags = UserDetails.None;
        if (modernMsg.IsModerator) udFlags |= UserDetails.Moderator;
        if (modernMsg.IsStaff) udFlags |= UserDetails.Staff;
        if (modernMsg.IsSubscriber) udFlags |= UserDetails.Subscriber;
        if (modernMsg.IsVip) udFlags |= UserDetails.Vip;
        if (modernMsg.Badges.Any(x => x.SetId.Contains("turbo", StringComparison.InvariantCultureIgnoreCase)))
            udFlags |= UserDetails.Turbo;
        if (modernMsg.Badges.Any(x => x.SetId.Contains("partner", StringComparison.InvariantCultureIgnoreCase)))
            udFlags |= UserDetails.Partner;

        if (_rules is TwitchRules { OnMessageReceived: { } legacyCallback })
            await legacyCallback(new(
                _client.TwitchUsername ?? "SubLink",
                modernMsg.ChatterUserId,
                modernMsg.ChatterUserLogin,
                modernMsg.ChatterUserName,
                modernMsg.Color,
                new("", modernMsg.Message.Text),
                modernMsg.Message.Text,
                modernMsgToUserType(),
                modernMsg.SourceBroadcasterUserName ?? modernMsg.BroadcasterUserName,
                modernMsg.MessageId,
                int.Parse(string.IsNullOrWhiteSpace(subBadgeInfo) ? "0" : subBadgeInfo),
                "",
                ChannelId!.Equals(modernMsg.ChatterUserId, StringComparison.InvariantCultureIgnoreCase),
                modernMsg.IsBroadcaster,
                Noisy.NotSet,
                modernMsg.Message.Text,
                modernMsg.Message.Text,
                badges,
                new(int.Parse(string.IsNullOrWhiteSpace(cheerBadgeInfo) ? "0" : cheerBadgeInfo)),
                bits,
                ConvertBitsToUsd(),
                new UserDetail(udFlags)
            ));
    }

    private async Task OnChannelCheer(object? sender, ChannelCheerArgs e) {
        if (_rules is TwitchRules { OnCheer: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelFollow(object? sender, ChannelFollowArgs e) {
        if (_rules is TwitchRules { OnFollow: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelHypeTrainBegin(object? sender, ChannelHypeTrainBeginV2Args e) {
        if (_rules is TwitchRules { OnHypeTrainBegin: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelHypeTrainEnd(object? sender, ChannelHypeTrainEndV2Args e) {
        if (_rules is TwitchRules { OnHypeTrainEnd: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelHypeTrainProgress(object? sender, ChannelHypeTrainProgressV2Args e) {
        if (_rules is TwitchRules { OnHypeTrainProgress: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelPointsCustomRewardRedemptionAdd(object? sender, ChannelPointsCustomRewardRedemptionArgs e) {
        if (_rules is TwitchRules { OnPointsCustomRewardRedemptionAdd: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelPointsCustomRewardRedemptionUpdate(object? sender, ChannelPointsCustomRewardRedemptionArgs e) {
        if (_rules is TwitchRules { OnPointsCustomRewardRedemptionUpdate: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPollBegin(object? sender, ChannelPollBeginArgs e) {
        if (_rules is TwitchRules { OnPollBegin: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPollEnd(object? sender, ChannelPollEndArgs e) {
        if (_rules is TwitchRules { OnPollEnd: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPollProgress(object? sender, ChannelPollProgressArgs e) {
        if (_rules is TwitchRules { OnPollProgress: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionBegin(object? sender, ChannelPredictionBeginArgs e) {
        if (_rules is TwitchRules { OnPredictionBegin: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionEnd(object? sender, ChannelPredictionEndArgs e) {
        if (_rules is TwitchRules { OnPredictionEnd: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionLock(object? sender, ChannelPredictionLockArgs e) {
        if (_rules is TwitchRules { OnPredictionLock: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelPredictionProgress(object? sender, ChannelPredictionProgressArgs e) {
        if (_rules is TwitchRules { OnPredictionProgress: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelRaid(object? sender, ChannelRaidArgs e) {
        if (_rules is TwitchRules { OnRaid: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelSubscribe(object? sender, ChannelSubscribeArgs e) {
        if (_rules is TwitchRules { OnSubscribe: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelSubscriptionEnd(object? sender, ChannelSubscriptionEndArgs e) {
        if (_rules is TwitchRules { OnSubscriptionEnd: { } callback })
            await callback(e.Payload.Event);

        _logger.Debug("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
    }

    private async Task OnChannelSubscriptionGift(object? sender, ChannelSubscriptionGiftArgs e) {
        if (_rules is TwitchRules { OnSubscriptionGift: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelSubscriptionMessage(object? sender, ChannelSubscriptionMessageArgs e) {
        if (_rules is TwitchRules { OnSubscriptionMessage: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnChannelUpdate(object? sender, ChannelUpdateArgs e) {
        if (_rules is TwitchRules { OnChannelUpdate: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnErrorOccurred(object? sender, ErrorOccuredArgs e) {
        _logger.Error(e.Exception, "[{TAG}] Websocket {EventSubSessionId}: {EMessage}",
            Platform.PlatformName, _eventSub.SessionId, e.Message);
        await Task.CompletedTask;
    }

    private async Task OnStreamOffline(object? sender, StreamOfflineArgs e) {
        if (_rules is TwitchRules { OnStreamOffline: { } callback })
            await callback(e.Payload.Event);
    }

    private async Task OnStreamOnline(object? sender, StreamOnlineArgs e) {
        if (_rules is TwitchRules { OnStreamOnline: { } callback })
            await callback(e.Payload.Event);
    }
}
