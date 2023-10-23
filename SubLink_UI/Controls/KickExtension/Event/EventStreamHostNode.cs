using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Stream Host"), Category("Kick/Event")]
internal class EventStreamHostNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarChatroomId,
        VarOptionalMessage,
        VarNumberViewers,
        VarHostUsername
    }

    public override string Title => "Kick Stream Host Event";

    public EventStreamHostNode() { }
    public EventStreamHostNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarChatroomId, "Chatroom ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarOptionalMessage, "Optional Message", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarNumberViewers, "Viewer Count", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarHostUsername, "Host Username", SlotType.VarOut, typeof(string));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not StreamHost)
            return;

        StreamHost msg = (StreamHost)para;
        SetValueInSlot((int)SlotId.VarChatroomId, msg.ChatroomId);
        SetValueInSlot((int)SlotId.VarOptionalMessage, msg.OptionalMessage);
        SetValueInSlot((int)SlotId.VarNumberViewers, msg.NumberViewers);
        SetValueInSlot((int)SlotId.VarHostUsername, msg.HostUsername);
    }

    protected override SequenceNode CopyImpl() =>
        new EventStreamHostNode();
}
