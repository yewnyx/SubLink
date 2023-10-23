using FlowGraph;
using FlowGraph.Node;
using System.Xml;

namespace tech.sublink.KickExtension.Event;

[Name("Pinned Message Deleted"), Category("Kick/Event")]
internal class EventPinnedMessageDeletedNode : EventNode {
    private enum SlotId {
        FlowOut
    }

    public override string Title => "Kick Pinned Message Deleted Event";

    public EventPinnedMessageDeletedNode() { }
    public EventPinnedMessageDeletedNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
    }

    protected override void TriggeredImpl(object? para) { }

    protected override SequenceNode CopyImpl() =>
        new EventPinnedMessageDeletedNode();
}
