using System;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Core.SubscriptionTypes.Stream;

namespace xyz.yewnyx.SubLink;

internal sealed class TwitchRules : ITwitchRules {
    internal Func<string, string, Task>? OnJoinedChannel;
    internal Func<ChatMessage, Task>? OnMessageReceived;
    internal Func<ChannelCheer, Task>? OnCheer;
    internal Func<ChannelFollow, Task>? OnFollow;
    internal Func<HypeTrainBegin, Task>? OnHypeTrainBegin;
    internal Func<HypeTrainEnd, Task>? OnHypeTrainEnd;
    internal Func<HypeTrainProgress, Task>? OnHypeTrainProgress;
    internal Func<ChannelPointsCustomRewardRedemption, Task>? OnPointsCustomRewardRedemptionAdd;
    internal Func<ChannelPointsCustomRewardRedemption, Task>? OnPointsCustomRewardRedemptionUpdate;
    internal Func<ChannelPollBegin, Task>? OnPollBegin;
    internal Func<ChannelPollEnd, Task>? OnPollEnd;
    internal Func<ChannelPollProgress, Task>? OnPollProgress;
    internal Func<ChannelPredictionBegin, Task>? OnPredictionBegin;
    internal Func<ChannelPredictionEnd, Task>? OnPredictionEnd;
    internal Func<ChannelPredictionLock, Task>? OnPredictionLock;
    internal Func<ChannelPredictionProgress, Task>? OnPredictionProgress;
    internal Func<ChannelRaid, Task>? OnRaid;
    internal Func<ChannelSubscribe, Task>? OnSubscribe;
    internal Func<ChannelSubscriptionEnd, Task>? OnSubscriptionEnd;
    internal Func<ChannelSubscriptionGift, Task>? OnSubscriptionGift;
    internal Func<ChannelSubscriptionMessage, Task>? OnSubscriptionMessage;
    internal Func<ChannelUpdate, Task>? OnChannelUpdate;
    internal Func<StreamOffline, Task>? OnStreamOffline;
    internal Func<StreamOnline, Task>? OnStreamOnline;

    void ITwitchRules.ReactToJoinedChannel(Func<string, string, Task> with) { OnJoinedChannel = with; }

    void ITwitchRules.ReactToMessageReceived(Func<ChatMessage, Task> with) { OnMessageReceived = with; }

    void ITwitchRules.ReactToCheer(Func<ChannelCheer, Task> with) { OnCheer = with; }

    void ITwitchRules.ReactToFollow(Func<ChannelFollow, Task> with) { OnFollow = with; }

    void ITwitchRules.ReactToHypeTrainBegin(Func<HypeTrainBegin, Task> with) { OnHypeTrainBegin = with; }

    void ITwitchRules.ReactToHypeTrainEnd(Func<HypeTrainEnd, Task> with) { OnHypeTrainEnd = with; }

    void ITwitchRules.ReactToHypeTrainProgress(Func<HypeTrainProgress, Task> with) { OnHypeTrainProgress = with; }

    void ITwitchRules.ReactToPointsCustomRewardRedemptionAdd(Func<ChannelPointsCustomRewardRedemption, Task> with) {
        OnPointsCustomRewardRedemptionAdd = with;
    }

    void ITwitchRules.ReactToPointsCustomRewardRedemptionUpdate(Func<ChannelPointsCustomRewardRedemption, Task> with) {
        OnPointsCustomRewardRedemptionUpdate = with;
    }

    void ITwitchRules.ReactToPollBegin(Func<ChannelPollBegin, Task> with) { OnPollBegin = with; }

    void ITwitchRules.ReactToPollEnd(Func<ChannelPollEnd, Task> with) { OnPollEnd = with; }

    void ITwitchRules.ReactToPollProgress(Func<ChannelPollProgress, Task> with) { OnPollProgress = with; }

    void ITwitchRules.ReactToPredictionBegin(Func<ChannelPredictionBegin, Task> with) { OnPredictionBegin = with; }

    void ITwitchRules.ReactToPredictionEnd(Func<ChannelPredictionEnd, Task> with) { OnPredictionEnd = with; }

    void ITwitchRules.ReactToPredictionLock(Func<ChannelPredictionLock, Task> with) { OnPredictionLock = with; }

    void ITwitchRules.ReactToPredictionProgress(Func<ChannelPredictionProgress, Task> with) {
        OnPredictionProgress = with;
    }

    void ITwitchRules.ReactToRaid(Func<ChannelRaid, Task> with) { OnRaid = with; }

    void ITwitchRules.ReactToSubscribe(Func<ChannelSubscribe, Task> with) { OnSubscribe = with; }

    void ITwitchRules.ReactToSubscriptionEnd(Func<ChannelSubscriptionEnd, Task> with) { OnSubscriptionEnd = with; }

    void ITwitchRules.ReactToSubscriptionGift(Func<ChannelSubscriptionGift, Task> with) { OnSubscriptionGift = with; }

    void ITwitchRules.ReactToSubscriptionMessage(Func<ChannelSubscriptionMessage, Task> with) {
        OnSubscriptionMessage = with;
    }

    void ITwitchRules.ReactToChannelUpdate(Func<ChannelUpdate, Task> with) { OnChannelUpdate = with; }

    void ITwitchRules.ReactToStreamOffline(Func<StreamOffline, Task> with) { OnStreamOffline = with; }

    void ITwitchRules.ReactToStreamOnline(Func<StreamOnline, Task> with) { OnStreamOnline = with; }
}