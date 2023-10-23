using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Chatroom Updated"), Category("Kick/Event")]
internal class EventChatroomUpdatedNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarChatroomId,
        VarSlowModeEnabled,
        VarSlowModeMessageInterval,
        VarSubscribersModeEnabled,
        VarFollowersModeEnabled,
        VarFollowersModeMinDuration,
        VarEmoteModeEnabled,
        VarAdvBotProtectModeEnabled,
        VarAdvBotProtectModeMessageInterval
    }

    public override string Title => "Kick Chatroom Updated Event";

    public EventChatroomUpdatedNode() { }
    public EventChatroomUpdatedNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarChatroomId, "Chatroom ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarSlowModeEnabled, "SlowMode Enabled", SlotType.VarOut, typeof(bool));
        AddSlot((int)SlotId.VarSlowModeMessageInterval, "Slow Mode Message Interval", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarSubscribersModeEnabled, "Subscribers Mode Enabled", SlotType.VarOut, typeof(bool));
        AddSlot((int)SlotId.VarFollowersModeEnabled, "Followers Mode Enabled", SlotType.VarOut, typeof(bool));
        AddSlot((int)SlotId.VarFollowersModeMinDuration, "Followers Mode Min Duration", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarEmoteModeEnabled, "Emote Mode Enabled", SlotType.VarOut, typeof(bool));
        AddSlot((int)SlotId.VarAdvBotProtectModeEnabled, "Advanced Bot Protection Mode Enabled", SlotType.VarOut, typeof(bool));
        AddSlot((int)SlotId.VarAdvBotProtectModeMessageInterval, "Advanced Bot Protection Mode Message Interval", SlotType.VarOut, typeof(uint));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not ChatroomUpdated)
            return;

        ChatroomUpdated msg = (ChatroomUpdated)para;
        SetValueInSlot((int)SlotId.VarChatroomId, msg.ChatroomId);
        SetValueInSlot((int)SlotId.VarSlowModeEnabled, msg.SlowMode.Enabled);
        SetValueInSlot((int)SlotId.VarSlowModeMessageInterval, msg.SlowMode.MessageInterval);
        SetValueInSlot((int)SlotId.VarSubscribersModeEnabled, msg.SubscribersMode.Enabled);
        SetValueInSlot((int)SlotId.VarFollowersModeEnabled, msg.FollowersMode.Enabled);
        SetValueInSlot((int)SlotId.VarFollowersModeMinDuration, msg.FollowersMode.MinDuration);
        SetValueInSlot((int)SlotId.VarEmoteModeEnabled, msg.EmotesMode.Enabled);
        SetValueInSlot((int)SlotId.VarAdvBotProtectModeEnabled, msg.AdvancedBotProtection.Enabled);
        SetValueInSlot((int)SlotId.VarAdvBotProtectModeMessageInterval, msg.AdvancedBotProtection.MessageInterval);
    }

    protected override SequenceNode CopyImpl() =>
        new EventChatroomUpdatedNode();
}
