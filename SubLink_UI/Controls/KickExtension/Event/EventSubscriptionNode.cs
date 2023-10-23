using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Subscription"), Category("Kick/Event")]
internal class EventSubscriptionNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarId,
        VarChatroomId,
        VarUsername,
        VarMonths
    }

    public override string Title => "Kick Subscription Event";

    public EventSubscriptionNode() { }
    public EventSubscriptionNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarId, "ID", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarChatroomId, "Chatroom ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarUsername, "Username", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarMonths, "Months", SlotType.VarOut, typeof(uint));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not Subscription)
            return;

        Subscription msg = (Subscription)para;
        SetValueInSlot((int)SlotId.VarId, msg.Id);
        SetValueInSlot((int)SlotId.VarChatroomId, msg.ChatroomId);
        SetValueInSlot((int)SlotId.VarUsername, msg.Username);
        SetValueInSlot((int)SlotId.VarMonths, msg.Months);
    }

    protected override SequenceNode CopyImpl() =>
        new EventSubscriptionNode();
}
