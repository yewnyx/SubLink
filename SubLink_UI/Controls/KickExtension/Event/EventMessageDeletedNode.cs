using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Message Deleted"), Category("Kick/Event")]
internal class EventMessageDeletedNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarId,
        VarMessageId
    }

    public override string Title => "Kick Message Deleted Event";

    public EventMessageDeletedNode() { }
    public EventMessageDeletedNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarId, "ID", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarMessageId, "Message ID", SlotType.VarOut, typeof(string));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not MessageDeleted)
            return;

        MessageDeleted msg = (MessageDeleted)para;
        SetValueInSlot((int)SlotId.VarId, msg.Id);
        SetValueInSlot((int)SlotId.VarMessageId, msg.Message.Id);
    }

    protected override SequenceNode CopyImpl() =>
        new EventMessageDeletedNode();
}
