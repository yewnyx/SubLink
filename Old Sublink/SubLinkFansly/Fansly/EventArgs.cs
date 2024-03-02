using System;
using xyz.yewnyx.SubLink.Fansly.Events;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class FanslyErrorArgs : EventArgs {
    public Exception Exception { get; set; } = new();

    public FanslyErrorArgs() { }

    public FanslyErrorArgs(Exception exception) {
        Exception = exception;
    }
}

public class FanslyEventArgs<T> : EventArgs where T : new() {
    public T Data { get; set; } = new T();
}

public sealed class ChatMessageEventArgs : FanslyEventArgs<ChatMessageEvent> { }

public sealed class TipEventArgs : FanslyEventArgs<TipEvent> { }

public sealed class GoalUpdatedEventArgs : FanslyEventArgs<GoalUpdatedEvent> { }
