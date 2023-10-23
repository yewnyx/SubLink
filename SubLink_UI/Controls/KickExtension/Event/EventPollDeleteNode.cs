using FlowGraph;
using FlowGraph.Node;
using System.Xml;

namespace tech.sublink.KickExtension.Event;

[Name("Poll Delete"), Category("Kick/Event")]
internal class EventPollDeleteNode : EventNode {
    private enum SlotId {
        FlowOut
    }

    public override string Title => "Kick Poll Delete Event";

    public EventPollDeleteNode() { }
    public EventPollDeleteNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
    }

    protected override void TriggeredImpl(object? para) { }

    protected override SequenceNode CopyImpl() =>
        new EventPollDeleteNode();
}
