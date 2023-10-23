using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Poll Update"), Category("Kick/Event")]
internal class EventPollUpdateNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarTitle,
        VarOptions,
        VarDuration,
        VarRemaining,
        VarResultDisplayDuration
    }

    public override string Title => "Kick Poll Update Event";

    public EventPollUpdateNode() { }
    public EventPollUpdateNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarTitle, "Title", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarOptions, "Options - Array<PollOptionInfo>", SlotType.VarOut, typeof(PollOptionInfo[]));
        AddSlot((int)SlotId.VarDuration, "Duration", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarRemaining, "Remaining", SlotType.VarOut, typeof(int));
        AddSlot((int)SlotId.VarResultDisplayDuration, "Result Display Duration", SlotType.VarOut, typeof(uint));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not PollUpdate)
            return;

        PollUpdate msg = (PollUpdate)para;
        SetValueInSlot((int)SlotId.VarTitle, msg.Poll.Title);
        SetValueInSlot((int)SlotId.VarOptions, msg.Poll.Options);
        SetValueInSlot((int)SlotId.VarDuration, msg.Poll.Duration);
        SetValueInSlot((int)SlotId.VarRemaining, msg.Poll.Remaining);
        SetValueInSlot((int)SlotId.VarResultDisplayDuration, msg.Poll.ResultDisplayDuration);
    }

    protected override SequenceNode CopyImpl() =>
        new EventPollUpdateNode();
}
