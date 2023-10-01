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

rules.Kick.ReactToSubscription(async sub => {
    logger.Information("Subscription {Username} subscribed for {Months} months", sub.Username, sub.Months);
});

rules.Kick.ReactToStreamHost(async streamHost => {
    logger.Information(
        "Hosting {HostUsername} with {NumberViewers} viewers",
        streamHost.HostUsername, streamHost.NumberViewers);
});

rules.Kick.ReactToUserBanned(async banned => {
    logger.Information(
        "User {UserUsername} got banned by {ModUsername} until {ExpiresAt}",
        banned.User.Username, banned.BannedBy.Username, banned.ExpiresAt);
});

rules.Kick.ReactToUserUnbanned(async unbanned => {
    logger.Information(
        "User {UserUsername} got unbanned by {ModUsername}",
        unbanned.User.Username, unbanned.UnbannedBy.Username);
});

rules.Kick.ReactToMessageDeleted(async deletedMessage => {
    logger.Information("Message ID {Id} got deleted", deletedMessage.Message.Id);
});

rules.Kick.ReactToChatroomClear(async chatroomClear => {
    logger.Information("Chatroom was cleared");
});

rules.Kick.ReactToChatroomUpdated(async chatroomUpdate => {
    logger.Information("Chatroom settings have been updated");
});

rules.Kick.ReactToPollUpdate(async pollUpdate => {
    logger.Information("Poll has been updated");
});

rules.Kick.ReactToPollDelete(async pollDelete => {
    logger.Information("Poll was deleted");
});

rules.Kick.ReactToPinnedMessageCreated(async pinnedMessage => {
    logger.Information(
        "Message ID {Id} has beenn pinned for {Duration} seconds",
        pinnedMessage.Message.Id, pinnedMessage.Duration);
});

rules.Kick.ReactToPinnedMessageDeleted(async pinnedMessage => {
    logger.Information("Pinned message was deleted");
});
