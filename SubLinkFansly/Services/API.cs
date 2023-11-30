using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using xyz.yewnyx.SubLink.Fansly.Events;

namespace xyz.yewnyx.SubLink; 

[PublicAPI]
public interface IFanslyRules {
    public void ReactToChatMessageEvent(Func<ChatMessageEvent, Task> with);
    public void ReactToTipEvent(Func<TipEvent, Task> with);
    public void ReactToGoalUpdatedEvent(Func<GoalUpdatedEvent, Task> with);
}