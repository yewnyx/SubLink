using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Core.SubscriptionTypes.Stream;
using xyz.yewnyx.SubLink.Platforms;

namespace xyz.yewnyx.SubLink.Twitch.Services;

[PublicAPI]
public sealed class TwitchRules : IPlatformRules {
    internal Func<string, string, Task>? OnJoinedChannel;
    internal Func<ChatMessage, Task>? OnMessageReceived;
    internal Func<ChannelBitsUse, Task>? OnChannelBitsUse;
    internal Func<ChannelCheer, Task>? OnCheer;
    internal Func<ChannelChatMessage, Task>? OnChatMessage;
    internal Func<ChannelFollow, Task>? OnFollow;
    internal Func<HypeTrainBeginV2, Task>? OnHypeTrainBegin;
    internal Func<HypeTrainEndV2, Task>? OnHypeTrainEnd;
    internal Func<HypeTrainProgressV2, Task>? OnHypeTrainProgress;
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

    public void ReactToJoinedChannel(Func<string, string, Task> with) { OnJoinedChannel = with; }

    public void ReactToMessageReceived(Func<ChatMessage, Task> with) { OnMessageReceived = with; }

    public void ReactToChannelBitsUse(Func<ChannelBitsUse, Task> with) { OnChannelBitsUse = with; }

    public void ReactToCheer(Func<ChannelCheer, Task> with) { OnCheer = with; }

    public void ReactToFollow(Func<ChannelFollow, Task> with) { OnFollow = with; }

    public void ReactToHypeTrainBegin(Func<HypeTrainBeginV2, Task> with) { OnHypeTrainBegin = with; }

    public void ReactToHypeTrainEnd(Func<HypeTrainEndV2, Task> with) { OnHypeTrainEnd = with; }

    public void ReactToHypeTrainProgress(Func<HypeTrainProgressV2, Task> with) { OnHypeTrainProgress = with; }

    public void ReactToPointsCustomRewardRedemptionAdd(Func<ChannelPointsCustomRewardRedemption, Task> with) { OnPointsCustomRewardRedemptionAdd = with; }

    public void ReactToPointsCustomRewardRedemptionUpdate(Func<ChannelPointsCustomRewardRedemption, Task> with) { OnPointsCustomRewardRedemptionUpdate = with; }

    public void ReactToPollBegin(Func<ChannelPollBegin, Task> with) { OnPollBegin = with; }

    public void ReactToPollEnd(Func<ChannelPollEnd, Task> with) { OnPollEnd = with; }

    public void ReactToPollProgress(Func<ChannelPollProgress, Task> with) { OnPollProgress = with; }

    public void ReactToPredictionBegin(Func<ChannelPredictionBegin, Task> with) { OnPredictionBegin = with; }

    public void ReactToPredictionEnd(Func<ChannelPredictionEnd, Task> with) { OnPredictionEnd = with; }

    public void ReactToPredictionLock(Func<ChannelPredictionLock, Task> with) { OnPredictionLock = with; }

    public void ReactToPredictionProgress(Func<ChannelPredictionProgress, Task> with) { OnPredictionProgress = with; }

    public void ReactToRaid(Func<ChannelRaid, Task> with) { OnRaid = with; }

    public void ReactToSubscribe(Func<ChannelSubscribe, Task> with) { OnSubscribe = with; }

    public void ReactToSubscriptionEnd(Func<ChannelSubscriptionEnd, Task> with) { OnSubscriptionEnd = with; }

    public void ReactToSubscriptionGift(Func<ChannelSubscriptionGift, Task> with) { OnSubscriptionGift = with; }

    public void ReactToSubscriptionMessage(Func<ChannelSubscriptionMessage, Task> with) { OnSubscriptionMessage = with; }

    public void ReactToChannelUpdate(Func<ChannelUpdate, Task> with) { OnChannelUpdate = with; }

    public void ReactToStreamOffline(Func<StreamOffline, Task> with) { OnStreamOffline = with; }

    public void ReactToStreamOnline(Func<StreamOnline, Task> with) { OnStreamOnline = with; }
}
