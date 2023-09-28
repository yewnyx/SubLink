using System;
using PusherClient;
using xyz.yewnyx.SubLink.Kick.Events;

namespace xyz.yewnyx.SubLink.Kick;

internal sealed class PusherConnectionStateChangedArgs : EventArgs {
    public ConnectionState State { get; set; } = ConnectionState.Disconnected;
}

internal sealed class PusherErrorArgs : EventArgs {
    public PusherException Exception { get; set; } = new PusherException(string.Empty, ErrorCodes.Unknown);
    public string Message { get; set; } = string.Empty;
}

internal sealed class PusherSubscribedArgs : EventArgs {
    public int SubscriptionCount { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
}

public class KickEventArgs<T> : EventArgs where T : new() {
    public T Data { get; set; } = new T();
}

public sealed class ChatMessageEventArgs : KickEventArgs<ChatMessageEvent> { }

public sealed class GiftedSubscriptionsEventArgs : KickEventArgs<GiftedSubscriptionsEvent> { }

public sealed class SubscriptionEventArgs : KickEventArgs<SubscriptionEvent> { }

public sealed class StreamHostEventArgs : KickEventArgs<StreamHostEvent> { }

public sealed class UserBannedEventArgs : KickEventArgs<UserBannedEvent> { }

public sealed class UserUnbannedEventArgs : KickEventArgs<UserUnbannedEvent> { }

public sealed class MessageDeletedEventArgs : KickEventArgs<MessageDeletedEvent> { }

public sealed class ChatroomClearEventArgs : KickEventArgs<ChatroomClearEvent> { }

public sealed class ChatroomUpdatedEventArgs : KickEventArgs<ChatroomUpdatedEvent> { }

public sealed class PollUpdateEventArgs : KickEventArgs<PollUpdateEvent> { }

public sealed class PollDeleteEventArgs : KickEventArgs<PollDeleteEvent> { }

public sealed class PinnedMessageCreatedEventArgs : KickEventArgs<PinnedMessageCreatedEvent> { }

public sealed class PinnedMessageDeletedEventArgs : KickEventArgs<PinnedMessageDeletedEvent> { }
