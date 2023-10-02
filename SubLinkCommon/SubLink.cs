using Serilog;
using System;
using System.Threading.Channels;

var notifier = new XSNotifier();

#if SUBLINK_TWITCH

logger.Information("Twitch integration enabled");

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

#endif
#if SUBLINK_KICK

logger.Information("Kick integration enabled");

kick.ReactToChatMessage(async chatMessage => {
    if ("yewnyx".Equals(chatMessage.Sender.Slug, StringComparison.InvariantCultureIgnoreCase)) {
        OscParameter.SendAvatarParameter("JacketToggle", false);
        OscParameter.SendAvatarParameter("Sus", true);
    }

    // Extract the Kick emote ID and send it to OSC. Format: [emote:37224:GIGACHAD]
    if (chatMessage.Content.Contains("[emote:")) {
        int idx = chatMessage.Content.IndexOf("[emote:") + 7;
        string emoteIdStr = chatMessage.Content.Substring(idx, chatMessage.Content.IndexOf(':', idx) - idx);

        if (int.TryParse(emoteIdStr, out int emoteId)) {
            OscParameter.SendAvatarParameter("emote", emoteId);
        }
    }

    logger.Information(
        "Username: {UserName}, Slug: {Slug}, Created At: {CreatedAt}, Content: {Content}",
        chatMessage.Sender.Username, chatMessage.Sender.Slug, chatMessage.CreatedAt, chatMessage.Content);
});

kick.ReactToGiftedSubscriptions(async giftedSubs => {
    logger.Information("Gifter {Gifter} gifted {GiftCount} subs", giftedSubs.Gifter, giftedSubs.GetGiftCount());

    switch(giftedSubs.GetGiftCount()) {
        case 2: {
            OscParameter.SendAvatarParameter("BoobaGrow", true);
            break;
        }
        case 3: {
            OscParameter.SendAvatarParameter("BulgeGrow", true);
            break;
        }
        case 5: {
            OscParameter.SendAvatarParameter("Outfit", 1);
            break;
        }
        case 6: {
            OscParameter.SendAvatarParameter("Outfit", 2);
            break;
        }
        default: break;
    }
});

kick.ReactToSubscription(async sub => {
    logger.Information("Subscription {Username} subscribed for {Months} months", sub.Username, sub.Months);
});

kick.ReactToStreamHost(async streamHost => {
    logger.Information(
        "Hosting {HostUsername} with {NumberViewers} viewers",
        streamHost.HostUsername, streamHost.NumberViewers);
});

kick.ReactToUserBanned(async banned => {
    logger.Information(
        "User {UserUsername} got banned by {ModUsername} until {ExpiresAt}",
        banned.User.Username, banned.BannedBy.Username, banned.ExpiresAt);
});

kick.ReactToUserUnbanned(async unbanned => {
    logger.Information(
        "User {UserUsername} got unbanned by {ModUsername}",
        unbanned.User.Username, unbanned.UnbannedBy.Username);
});

kick.ReactToMessageDeleted(async deletedMessage => {
    logger.Information("Message ID {Id} got deleted", deletedMessage.Message.Id);
});

kick.ReactToChatroomClear(async chatroomClear => {
    logger.Information("Chatroom was cleared");
});

kick.ReactToChatroomUpdated(async chatroomUpdate => {
    logger.Information("Chatroom settings have been updated");
});

kick.ReactToPollUpdate(async pollUpdate => {
    logger.Information("Poll has been updated");
});

kick.ReactToPollDelete(async pollDelete => {
    logger.Information("Poll was deleted");
});

kick.ReactToPinnedMessageCreated(async pinnedMessage => {
    logger.Information(
        "Message ID {Id} has beenn pinned for {Duration} seconds",
        pinnedMessage.Message.Id, pinnedMessage.Duration);
});

kick.ReactToPinnedMessageDeleted(async pinnedMessage => {
    logger.Information("Pinned message was deleted");
});

#endif