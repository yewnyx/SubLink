using Serilog;
using System;
using System.Threading.Channels;

logger.Information("Test (delete me)");

var notifier = new XSNotifier();

rules.Kick.ReactToChatMessage(async chatMessage => {
    if ("yewnyx".Equals(chatMessage.Sender.Slug, StringComparison.InvariantCultureIgnoreCase)) {
        OscParameter.SendAvatarParameter("JacketToggle", false);
        OscParameter.SendAvatarParameter("Sus", true);
    }
    logger.Information(
        "Username: {UserName}, Slug: {Slug}, Created At: {CreatedAt}, Content: {Content}",
        chatMessage.Sender.Username, chatMessage.Sender.Slug, chatMessage.CreatedAt, chatMessage.Content);
});

rules.Kick.ReactToGiftedSubscriptions(async giftedSubs => {
    logger.Information("Gifter {Gifter} gifted {GiftCount} subs", giftedSubs.Gifter, giftedSubs.GetGiftCount());
});

/*
rules.Twitch.ReactToRaid(async channelRaid => {
    logger.Information("{Login} raided you with {ViewerCount} viewers", channelRaid.FromBroadcasterUserName, channelRaid.Viewers);
});

rules.Twitch.ReactToSubscribe(async channelSubscribe => {
    logger.Information("New subscription: User {UserName} ({Login}) {WasGift} at tier \"{Tier}\" ",
        channelSubscribe.UserName, channelSubscribe.UserLogin, channelSubscribe.IsGift ? "was gifted a sub" : "subscribed", channelSubscribe.Tier);
});

rules.Twitch.ReactToSubscriptionGift(async channelSubscriptionGift => {
    logger.Information(
        "User {UserName} ({Login}) gifted {Total} \"{Tier}\" subs - they have gifted {CumulativeTotal} subs total",
        channelSubscriptionGift.UserName, channelSubscriptionGift.UserId, channelSubscriptionGift.Total, channelSubscriptionGift.Tier, channelSubscriptionGift.CumulativeTotal);
});

rules.Twitch.ReactToSubscriptionMessage(async channelSubscriptionMessage => {
    logger.Information(
        "User {UserName} ({Login}) resubscribed at tier \"{Tier}\" for {Months} months (streak: {Streak}) with message: \"{Message}\"",
        channelSubscriptionMessage.UserName, channelSubscriptionMessage.UserLogin, channelSubscriptionMessage.Tier, channelSubscriptionMessage.DurationMonths, channelSubscriptionMessage.StreakMonths, channelSubscriptionMessage.Message);
});

rules.Twitch.ReactToChannelUpdate(async channelUpdate => {
    logger.Information("Streamer {UserName} updated with title: \"{Title}\" category: \"{category}\"", channelUpdate.BroadcasterUserName, channelUpdate.CategoryName, channelUpdate.Title);
});

rules.Twitch.ReactToStreamOffline(async streamOffline => {
    logger.Information("Streamer {UserName} stopped streaming", streamOffline.BroadcasterUserName);
});

rules.Twitch.ReactToStreamOnline(async streamOnline => {
    logger.Information("Streamer {UserName} started a {Type} stream at {StartedAt}", streamOnline.BroadcasterUserName, streamOnline.Type, streamOnline.StartedAt.ToString("F"));
});
*/