using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Pinned Message Created"), Category("Kick/Event")]
internal class EventPinnedMessageCreatedNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarMessageId,
        VarMessageChatroomId,
        VarMessageContent,
        VarMessageType,
        VarMessageCreatedAt,
        VarMessageSender,
        //VarMessageMetadata,
        VarDuration
    }

    public override string Title => "Kick Pinned Message Created Event";

    public EventPinnedMessageCreatedNode() { }
    public EventPinnedMessageCreatedNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarMessageId, "Message ID", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarMessageChatroomId, "Message Chatroom ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarMessageContent, "Message Content", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarMessageType, "Message Type", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarMessageCreatedAt, "Message Created At", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarMessageSender, "Message Sender - KickUser", SlotType.VarOut, typeof(KickUser));
        //AddSlot((int)SlotId.VarMessageMetadata, "Message Metadata", SlotType.VarOut, typeof(object));
        AddSlot((int)SlotId.VarDuration, "Duration", SlotType.VarOut, typeof(string));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not PinnedMessageCreated)
            return;

        PinnedMessageCreated msg = (PinnedMessageCreated)para;
        SetValueInSlot((int)SlotId.VarMessageId, msg.Message.Id);
        SetValueInSlot((int)SlotId.VarMessageChatroomId, msg.Message.ChatroomId);
        SetValueInSlot((int)SlotId.VarMessageContent, msg.Message.Content);
        SetValueInSlot((int)SlotId.VarMessageType, msg.Message.Type);
        SetValueInSlot((int)SlotId.VarMessageCreatedAt, msg.Message.CreatedAt);
        SetValueInSlot((int)SlotId.VarMessageSender, msg.Message.Sender);
        //SetValueInSlot((int)SlotId.VarMessageMetadata, msg.Message.Metadata);
        SetValueInSlot((int)SlotId.VarDuration, msg.Duration);
    }

    protected override SequenceNode CopyImpl() =>
        new EventPinnedMessageCreatedNode();
}
