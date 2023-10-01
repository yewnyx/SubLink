using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using xyz.yewnyx.SubLink.Kick.Events;

namespace xyz.yewnyx.SubLink; 

[PublicAPI]
public interface IKickRules {
    public void ReactToChatMessage(Func<ChatMessageEvent, Task> with);
    public void ReactToGiftedSubscriptions(Func<GiftedSubscriptionsEvent, Task> with);
    public void ReactToSubscription(Func<SubscriptionEvent, Task> with);
    public void ReactToStreamHost(Func<StreamHostEvent, Task> with);
    public void ReactToUserBanned(Func<UserBannedEvent, Task> with);
    public void ReactToUserUnbanned(Func<UserUnbannedEvent, Task> with);
    public void ReactToMessageDeleted(Func<MessageDeletedEvent, Task> with);
    public void ReactToChatroomClear(Func<ChatroomClearEvent, Task> with);
    public void ReactToChatroomUpdated(Func<ChatroomUpdatedEvent, Task> with);
    public void ReactToPollUpdate(Func<PollUpdateEvent, Task> with);
    public void ReactToPollDelete(Func<PollDeleteEvent, Task> with);
    public void ReactToPinnedMessageCreated(Func<PinnedMessageCreatedEvent, Task> with);
    public void ReactToPinnedMessageDeleted(Func<PinnedMessageDeletedEvent, Task> with);
}