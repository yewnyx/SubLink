using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Fansly;
using xyz.yewnyx.SubLink.Fansly.Events;

namespace xyz.yewnyx.SubLink;

internal sealed class FanslyRules : IFanslyRules {
    internal Func<ChatMessageEvent, Task>? OnChatMessageEvent;
    internal Func<TipEvent, Task>? OnTipEvent;
    internal Func<GoalUpdatedEvent, Task>? OnGoalUpdatedEvent;

    void IFanslyRules.ReactToChatMessageEvent(Func<ChatMessageEvent, Task> with) { OnChatMessageEvent = with; }
    void IFanslyRules.ReactToTipEvent(Func<TipEvent, Task> with) { OnTipEvent = with; }
    void IFanslyRules.ReactToGoalUpdatedEvent(Func<GoalUpdatedEvent, Task> with) { OnGoalUpdatedEvent = with; }
}