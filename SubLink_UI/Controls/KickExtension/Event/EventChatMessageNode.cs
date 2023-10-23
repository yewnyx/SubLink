using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Chat Message"), Category("Event/Kick")]
public class EventChatMessageNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarId,
        VarChatroomId,
        VarContent,
        VarType,
        VarCreatedAt,
        VarSender
    }

    public override string Title => "Kick Chat Message Event";

    public EventChatMessageNode() { }
    public EventChatMessageNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarId, "ID", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarChatroomId, "Chatroom ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarContent, "Content", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarType, "Type", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarCreatedAt, "Created At", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarSender, "Sender KickUser", SlotType.VarOut, typeof(KickUser));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not ChatMessage)
            return;

        ChatMessage msg = (ChatMessage)para;
        SetValueInSlot((int)SlotId.VarId, msg.Id);
        SetValueInSlot((int)SlotId.VarChatroomId, msg.ChatroomId);
        SetValueInSlot((int)SlotId.VarContent, msg.Content);
        SetValueInSlot((int)SlotId.VarType, msg.Type);
        SetValueInSlot((int)SlotId.VarCreatedAt, msg.CreatedAt);
        SetValueInSlot((int)SlotId.VarSender, msg.Sender);
    }

    protected override SequenceNode CopyImpl() =>
        new EventChatMessageNode();
}
