using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TwitchLib.Client.Models;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Core.SubscriptionTypes.Stream;

namespace xyz.yewnyx.SubLink; 

[PublicAPI]
public interface IRules {
    ITwitchRules Twitch { get; }
}

[PublicAPI]
public interface ITwitchRules {
    public void ReactToJoinedChannel(Func<string, string, Task> with);
    public void ReactToMessageReceived(Func<ChatMessage, Task> with);
    public void ReactToCheer(Func<ChannelCheer, Task> with);
    public void ReactToFollow(Func<ChannelFollow, Task> with);
    public void ReactToHypeTrainBegin(Func<HypeTrainBegin, Task> with);
    public void ReactToHypeTrainEnd(Func<HypeTrainEnd, Task> with);
    public void ReactToHypeTrainProgress(Func<HypeTrainProgress, Task> with);
    public void ReactToPointsCustomRewardRedemptionAdd(Func<ChannelPointsCustomRewardRedemption, Task> with);
    public void ReactToPointsCustomRewardRedemptionUpdate(Func<ChannelPointsCustomRewardRedemption, Task> with);
    public void ReactToPollBegin(Func<ChannelPollBegin, Task> with);
    public void ReactToPollEnd(Func<ChannelPollEnd, Task> with);
    public void ReactToPollProgress(Func<ChannelPollProgress, Task> with);
    public void ReactToPredictionBegin(Func<ChannelPredictionBegin, Task> with);
    public void ReactToPredictionEnd(Func<ChannelPredictionEnd, Task> with);
    public void ReactToPredictionLock(Func<ChannelPredictionLock, Task> with);
    public void ReactToPredictionProgress(Func<ChannelPredictionProgress, Task> with);
    public void ReactToRaid(Func<ChannelRaid, Task> with);
    public void ReactToSubscribe(Func<ChannelSubscribe, Task> with);
    public void ReactToSubscriptionEnd(Func<ChannelSubscriptionEnd, Task> with);
    public void ReactToSubscriptionGift(Func<ChannelSubscriptionGift, Task> with);
    public void ReactToSubscriptionMessage(Func<ChannelSubscriptionMessage, Task> with);
    public void ReactToChannelUpdate(Func<ChannelUpdate, Task> with);
    public void ReactToStreamOffline(Func<StreamOffline, Task> with);
    public void ReactToStreamOnline(Func<StreamOnline, Task> with);
}