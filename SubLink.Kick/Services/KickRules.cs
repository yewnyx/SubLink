using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Kick.KickClient.Events;
using xyz.yewnyx.SubLink.Platforms;

namespace xyz.yewnyx.SubLink.Kick.Services;

[PublicAPI]
public sealed class KickRules : IPlatformRules {
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

    public void ReactToChatMessage(Func<ChatMessageEvent, Task> with) { OnChatMessage = with; }
    public void ReactToGiftedSubscriptions(Func<GiftedSubscriptionsEvent, Task> with) { OnGiftedSubscriptions = with; }
    public void ReactToSubscription(Func<SubscriptionEvent, Task> with) { OnSubscription = with; }
    public void ReactToStreamHost(Func<StreamHostEvent, Task> with) { OnStreamHost = with; }
    public void ReactToUserBanned(Func<UserBannedEvent, Task> with) { OnUserBanned = with; }
    public void ReactToUserUnbanned(Func<UserUnbannedEvent, Task> with) { OnUserUnbanned = with; }
    public void ReactToMessageDeleted(Func<MessageDeletedEvent, Task> with) { OnMessageDeleted = with; }
    public void ReactToChatroomClear(Func<ChatroomClearEvent, Task> with) { OnChatroomClear = with; }
    public void ReactToChatroomUpdated(Func<ChatroomUpdatedEvent, Task> with) { OnChatroomUpdated = with; }
    public void ReactToPollUpdate(Func<PollUpdateEvent, Task> with) { OnPollUpdate = with; }
    public void ReactToPollDelete(Func<EventArgs, Task> with) { OnPollDelete = with; }
    public void ReactToPinnedMessageCreated(Func<PinnedMessageCreatedEvent, Task> with) { OnPinnedMessageCreated = with; }
    public void ReactToPinnedMessageDeleted(Func<EventArgs, Task> with) { OnPinnedMessageDeleted = with; }
}
