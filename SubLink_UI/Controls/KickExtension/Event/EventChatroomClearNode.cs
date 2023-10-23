using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Chatroom Clear"), Category("Kick/Event")]
internal class EventChatroomClearNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarId
    }

    public override string Title => "Kick Chat Clear Event";

    public EventChatroomClearNode() { }
    public EventChatroomClearNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarId, "ID", SlotType.VarOut, typeof(string));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not ChatroomClear)
            return;

        ChatroomClear msg = (ChatroomClear)para;
        SetValueInSlot((int)SlotId.VarId, msg.Id);
    }

    protected override SequenceNode CopyImpl() =>
        new EventChatroomClearNode();
}
