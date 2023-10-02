using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Kick.Events;

namespace xyz.yewnyx.SubLink;

internal sealed class KickRules : IKickRules {
    internal Func<ChatMessageEvent, Task>? OnChatMessage;
    internal Func<GiftedSubscriptionsEvent, Task>? OnGiftedSubscriptions;
    internal Func<SubscriptionEvent, Task>? OnSubscription;
    internal Func<StreamHostEvent, Task>? OnStreamHost;
    internal Func<UserBannedEvent, Task>? OnUserBanned;
    internal Func<UserUnbannedEvent, Task>? OnUserUnbanned;
    internal Func<MessageDeletedEvent, Task>? OnMessageDeleted;
    internal Func<ChatroomClearEvent, Task>? OnChatroomClear;
    internal Func<ChatroomUpdatedEvent, Task>? OnChatroomUpdated;
    internal Func<PollUpdateEvent, Task>? OnPollUpdate;
    internal Func<EventArgs, Task>? OnPollDelete;
    internal Func<PinnedMessageCreatedEvent, Task>? OnPinnedMessageCreated;
    internal Func<EventArgs, Task>? OnPinnedMessageDeleted;

    void IKickRules.ReactToChatMessage(Func<ChatMessageEvent, Task> with) { OnChatMessage = with; }
    void IKickRules.ReactToGiftedSubscriptions(Func<GiftedSubscriptionsEvent, Task> with) {
        OnGiftedSubscriptions = with;
    }
    void IKickRules.ReactToSubscription(Func<SubscriptionEvent, Task> with) { OnSubscription = with; }
    void IKickRules.ReactToStreamHost(Func<StreamHostEvent, Task> with) { OnStreamHost = with; }
    void IKickRules.ReactToUserBanned(Func<UserBannedEvent, Task> with) { OnUserBanned = with; }
    void IKickRules.ReactToUserUnbanned(Func<UserUnbannedEvent, Task> with) { OnUserUnbanned = with; }
    void IKickRules.ReactToMessageDeleted(Func<MessageDeletedEvent, Task> with) { OnMessageDeleted = with; }
    void IKickRules.ReactToChatroomClear(Func<ChatroomClearEvent, Task> with) { OnChatroomClear = with; }
    void IKickRules.ReactToChatroomUpdated(Func<ChatroomUpdatedEvent, Task> with) { OnChatroomUpdated = with; }
    void IKickRules.ReactToPollUpdate(Func<PollUpdateEvent, Task> with) { OnPollUpdate = with; }
    void IKickRules.ReactToPollDelete(Func<EventArgs, Task> with) { OnPollDelete = with; }
    void IKickRules.ReactToPinnedMessageCreated(Func<PinnedMessageCreatedEvent, Task> with) {
        OnPinnedMessageCreated = with;
    }
    void IKickRules.ReactToPinnedMessageDeleted(Func<EventArgs, Task> with) {
        OnPinnedMessageDeleted = with;
    }
}