using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("User Unbanned"), Category("Kick/Event")]
internal class EventUserUnbannedNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarId,
        VarUser,
        VarUnbannedBy
    }

    public override string Title => "Kick User Unbanned Event";

    public EventUserUnbannedNode() { }
    public EventUserUnbannedNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarId, "ID", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarUser, "User - KickUserShort", SlotType.VarOut, typeof(KickUserShort));
        AddSlot((int)SlotId.VarUnbannedBy, "Unbanned By - KickUserShort", SlotType.VarOut, typeof(KickUserShort));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not UserUnbanned)
            return;

        UserUnbanned msg = (UserUnbanned)para;
        SetValueInSlot((int)SlotId.VarId, msg.Id);
        SetValueInSlot((int)SlotId.VarUser, msg.User);
        SetValueInSlot((int)SlotId.VarUnbannedBy, msg.UnbannedBy);
    }

    protected override SequenceNode CopyImpl() =>
        new EventUserUnbannedNode();
}
