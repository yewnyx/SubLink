using Serilog;
using System;
using System.Threading.Channels;

logger.Information("Test (delete me)");

var notifier = new XSNotifier();

twitch.ReactToJoinedChannel(async (channel, botUsername) => {
    logger.Information("User {BotUsername} joined channel {ChannelName}", botUsername, channel);
});

twitch.ReactToMessageReceived(async chatMessage => {
    if ("yewnyx".Equals(chatMessage.Username, StringComparison.InvariantCultureIgnoreCase)) {
        OscParameter.SendAvatarParameter("JacketToggle", false);
        OscParameter.SendAvatarParameter("Sus", true);
    }
    logger.Information(
        "Username: {UserName}, Display name: {DisplayName}, User Type: {UserType}, Message: {Message}",
        chatMessage.Username, chatMessage.DisplayName, chatMessage.UserType, chatMessage.Message);
});

twitch.ReactToCheer(async channelCheer => {
    logger.Information(
        "{UserName} cheered {Bits} bits to {BroadcasterUserName} with {Message}",
        channelCheer.UserName, channelCheer.Bits, channelCheer.BroadcasterUserName, channelCheer.Message);
});

twitch.ReactToFollow(async follow => {
    logger.Information(
        "{EventDataUserName} followed {EventDataBroadcasterUserName} at {EventDataFollowedAt}",
        follow.UserName, follow.BroadcasterUserName, follow.FollowedAt);
});

twitch.ReactToHypeTrainBegin(async hypeTrainBegin => {
    logger.Information(
        "Hype Train for {BroadcasterUserName} began at level {Level}. Progress: {Progress} - {Goal} to next level. Total Points: {Total}",
        hypeTrainBegin.BroadcasterUserName, hypeTrainBegin.Level, hypeTrainBegin.Progress, hypeTrainBegin.Goal, hypeTrainBegin.Total);
});

twitch.ReactToHypeTrainEnd(async hypeTrainEnd => {
    logger.Information(
        "Hype Train for {BroadcasterUserName} ended at level {Level}. Total Points: {Total}",
        hypeTrainEnd.BroadcasterUserName, hypeTrainEnd.Level, hypeTrainEnd.Total);
});

twitch.ReactToHypeTrainProgress(async hypeTrainProgress => {
    logger.Information(
        "Hype Train for {BroadcasterUserName} progressed to level {Level}. Progress: {Progress} - {Goal} to next level. Total Points: {Total}",
        hypeTrainProgress.BroadcasterUserName, hypeTrainProgress.Level, hypeTrainProgress.Progress, hypeTrainProgress.Goal, hypeTrainProgress.Total);
});

twitch.ReactToPointsCustomRewardRedemptionAdd(async channelPointsCustomRewardRedemption => {
    var r = channelPointsCustomRewardRedemption;
            
    logger.Information("User {DisplayName} ({Login}) redeemed {RewardId} {Title} with message {UserInput}", 
        r.UserName, r.UserLogin, r.Reward.Id, r.Reward.Title, r.UserInput);
});

twitch.ReactToRaid(async channelRaid => {
    logger.Information("{Login} raided you with {ViewerCount} viewers", channelRaid.FromBroadcasterUserName, channelRaid.Viewers);
});

twitch.ReactToSubscribe(async channelSubscribe => {
    logger.Information("New subscription: User {UserName} ({Login}) {WasGift} at tier \"{Tier}\" ",
        channelSubscribe.UserName, channelSubscribe.UserLogin, channelSubscribe.IsGift ? "was gifted a sub" : "subscribed", channelSubscribe.Tier);
});

twitch.ReactToSubscriptionGift(async channelSubscriptionGift => {
    logger.Information(
        "User {UserName} ({Login}) gifted {Total} \"{Tier}\" subs - they have gifted {CumulativeTotal} subs total",
        channelSubscriptionGift.UserName, channelSubscriptionGift.UserId, channelSubscriptionGift.Total, channelSubscriptionGift.Tier, channelSubscriptionGift.CumulativeTotal);
});

twitch.ReactToSubscriptionMessage(async channelSubscriptionMessage => {
    logger.Information(
        "User {UserName} ({Login}) resubscribed at tier \"{Tier}\" for {Months} months (streak: {Streak}) with message: \"{Message}\"",
        channelSubscriptionMessage.UserName, channelSubscriptionMessage.UserLogin, channelSubscriptionMessage.Tier, channelSubscriptionMessage.DurationMonths, channelSubscriptionMessage.StreakMonths, channelSubscriptionMessage.Message);
});

twitch.ReactToChannelUpdate(async channelUpdate => {
    logger.Information("Streamer {UserName} updated with title: \"{Title}\" category: \"{category}\"", channelUpdate.BroadcasterUserName, channelUpdate.CategoryName, channelUpdate.Title);
});

twitch.ReactToStreamOffline(async streamOffline => {
    logger.Information("Streamer {UserName} stopped streaming", streamOffline.BroadcasterUserName);
});

twitch.ReactToStreamOnline(async streamOnline => {
    logger.Information("Streamer {UserName} started a {Type} stream at {StartedAt}", streamOnline.BroadcasterUserName, streamOnline.Type, streamOnline.StartedAt.ToString("F"));
});