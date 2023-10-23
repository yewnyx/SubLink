using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("Gifted Subscriptions"), Category("Kick/Event")]
internal class EventGiftedSubscriptionsNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarId,
        VarChatroomId,
        VarUsers,
        VarGifter,
        VarGiftedCount
    }

    public override string Title => "Kick Gifted Subscriptions Event";

    public EventGiftedSubscriptionsNode() { }
    public EventGiftedSubscriptionsNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarId, "ID", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarChatroomId, "Chatroom ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)SlotId.VarUsers, "Users - Array<string>", SlotType.VarOut, typeof(string[]));
        AddSlot((int)SlotId.VarGifter, "Gifter", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarGiftedCount, "Gifted Count", SlotType.VarOut, typeof(int));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not GiftedSubscriptions)
            return;

        GiftedSubscriptions msg = (GiftedSubscriptions)para;
        SetValueInSlot((int)SlotId.VarId, msg.Id);
        SetValueInSlot((int)SlotId.VarChatroomId, msg.ChatroomId);
        SetValueInSlot((int)SlotId.VarUsers, msg.Users);
        SetValueInSlot((int)SlotId.VarGifter, msg.Gifter);
        SetValueInSlot((int)SlotId.VarGiftedCount, msg.GetGiftCount());
    }

    protected override SequenceNode CopyImpl() =>
        new EventGiftedSubscriptionsNode();
}
