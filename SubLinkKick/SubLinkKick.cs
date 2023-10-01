using Serilog;
using System;
using System.Threading.Channels;

logger.Information("Test (delete me)");

var notifier = new XSNotifier();

kick.ReactToChatMessage(async chatMessage => {
    if ("yewnyx".Equals(chatMessage.Sender.Slug, StringComparison.InvariantCultureIgnoreCase)) {
        OscParameter.SendAvatarParameter("JacketToggle", false);
        OscParameter.SendAvatarParameter("Sus", true);
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
