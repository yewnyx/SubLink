using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using xyz.yewnyx.SubLink.Fansly.Events;

namespace xyz.yewnyx.SubLink; 

[PublicAPI]
public interface IFanslyRules {
    public void ReactToChatMessage(Func<ChatMessageEvent, Task> with);
    public void ReactToTip(Func<TipEvent, Task> with);
    public void ReactToGoalUpdated(Func<GoalUpdatedEvent, Task> with);
}