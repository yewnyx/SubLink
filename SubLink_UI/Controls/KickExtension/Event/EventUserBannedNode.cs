using FlowGraph;
using FlowGraph.Node;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Event;

[Name("User Banned"), Category("Kick/Event")]
internal class EventUserBannedNode : EventNode {
    private enum SlotId {
        FlowOut,
        VarId,
        VarUser,
        VarBannedBy,
        VarExpiresAt
    }

    public override string Title => "Kick User Banned Event";

    public EventUserBannedNode() { }
    public EventUserBannedNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)SlotId.FlowOut, "Triggered", SlotType.NodeOut);
        AddSlot((int)SlotId.VarId, "ID", SlotType.VarOut, typeof(string));
        AddSlot((int)SlotId.VarUser, "User - KickUserShort", SlotType.VarOut, typeof(KickUserShort));
        AddSlot((int)SlotId.VarBannedBy, "Banned By - KickUserShort", SlotType.VarOut, typeof(KickUserShort));
        AddSlot((int)SlotId.VarExpiresAt, "Expires At", SlotType.VarOut, typeof(string));
    }

    protected override void TriggeredImpl(object? para) {
        if (para == null || para is not UserBanned)
            return;

        UserBanned msg = (UserBanned)para;
        SetValueInSlot((int)SlotId.VarId, msg.Id);
        SetValueInSlot((int)SlotId.VarUser, msg.User);
        SetValueInSlot((int)SlotId.VarBannedBy, msg.BannedBy);
        SetValueInSlot((int)SlotId.VarExpiresAt, msg.ExpiresAt);
    }

    protected override SequenceNode CopyImpl() =>
        new EventUserBannedNode();
}
