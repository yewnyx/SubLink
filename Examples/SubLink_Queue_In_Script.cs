using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Timers;
using VRC.OSCQuery;

const int CQueueTimer = 100; // Process queues every 1/10th of a second

OSCParamStatus<int> twitchSubscriptionQueue = new((subMonths) => {
    OscParameter.SendAvatarParameter("TwitchSubscription", subMonths);
});
OSCParamStatus<int> twitchCommunityGiftQueue = new((amount) => {
    OscParameter.SendAvatarParameter("TwitchCommunityGift", amount);
});

var notifier = new XSNotifier();
var queueTimer = new System.Timers.Timer(CQueueTimer) {
    AutoReset = true,
    Enabled = true
};
queueTimer.Elapsed += OnQueueTimer;

void OnQueueTimer(object? source, ElapsedEventArgs e) {
    if (twitchSubscriptionQueue.IsIdle && twitchSubscriptionQueue.ValueQueue.Count > 0) {
        twitchSubscriptionQueue.IsIdle = false;
        twitchSubscriptionQueue.HandleQueueItem(twitchSubscriptionQueue.ValueQueue.Dequeue());
    }

    if (twitchCommunityGiftQueue.IsIdle && twitchCommunityGiftQueue.ValueQueue.Count > 0) {
        twitchCommunityGiftQueue.IsIdle = false;
        twitchCommunityGiftQueue.HandleQueueItem(twitchCommunityGiftQueue.ValueQueue.Dequeue());
    }
}

oscQuery.AddEndpoint<bool>("/avatar/parameters/TwitchSubscription", Attributes.AccessValues.ReadWrite, new object[] { 0 });
oscServer.TryAddMethod("/avatar/parameters/TwitchSubscription", message => {
    int curVal = message.ReadIntElement(0);

    if (curVal == 0)
        twitchSubscriptionQueue.IsIdle = true;
});

oscQuery.AddEndpoint<bool>("/avatar/parameters/TwitchCommunityGift", Attributes.AccessValues.ReadWrite, new object[] { 0 });
oscServer.TryAddMethod("/avatar/parameters/TwitchCommunityGift", message => {
    int curVal = message.ReadIntElement(0);

    if (curVal == 0)
        twitchCommunityGiftQueue.IsIdle = true;
});

#if SUBLINK_TWITCH

logger.Information("Twitch integration enabled");

twitch.ReactToSubscribe(async channelSubscribe => {
    logger.Information("New subscription: User {UserName} ({Login}) {WasGift} at tier \"{Tier}\" ",
        channelSubscribe.UserName, channelSubscribe.UserLogin, channelSubscribe.IsGift ? "was gifted a sub" : "subscribed", channelSubscribe.Tier);

    if (!channelSubscribe.IsGift)
        twitchSubscriptionQueue.ValueQueue.Enqueue(1);
});

twitch.ReactToSubscriptionMessage(async channelSubscriptionMessage => {
    logger.Information(
        "User {UserName} ({Login}) resubscribed at tier \"{Tier}\" for {Months} months (streak: {Streak}) with message: \"{Message}\"",
        channelSubscriptionMessage.UserName, channelSubscriptionMessage.UserLogin, channelSubscriptionMessage.Tier, channelSubscriptionMessage.DurationMonths, channelSubscriptionMessage.StreakMonths, channelSubscriptionMessage.Message);
    twitchSubscriptionQueue.ValueQueue.Enqueue(channelSubscriptionMessage.DurationMonths);
});

twitch.ReactToSubscriptionGift(async channelSubscriptionGift => {
    logger.Information(
        "User {UserName} ({Login}) gifted {Total} \"{Tier}\" subs - they have gifted {CumulativeTotal} subs total",
        channelSubscriptionGift.UserName, channelSubscriptionGift.UserId, channelSubscriptionGift.Total, channelSubscriptionGift.Tier, channelSubscriptionGift.CumulativeTotal);
    twitchCommunityGiftQueue.ValueQueue.Enqueue(channelSubscriptionGift.Total);
});

#endif

class OSCParamStatus<T> {
    public bool IsIdle { get; set; } = true;

    public Queue<T> ValueQueue { get; } = new();

    public Action<T> HandleQueueItem { get; private set; }

    public OSCParamStatus(Action<T> handler) {
        HandleQueueItem = handler;
    }
}
