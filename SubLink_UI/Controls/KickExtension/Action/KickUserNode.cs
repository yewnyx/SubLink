using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node;
using FlowGraph.Process;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Action;

[Name("KickUser"), Category("Objects/Kick")]
public class KickUserNode : ActionNode {
    private enum NodeSlotId {
        FlowIn,
        FlowOut,
        VarKickUser,
        VarId,
        VarUsername,
        VarSlug,
        VarIdentity
    }

    public override string? Title => "KickUser";

    public KickUserNode() { }
    public KickUserNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();
        AddSlot((int)NodeSlotId.FlowIn, string.Empty, SlotType.NodeIn);
        AddSlot((int)NodeSlotId.FlowOut, string.Empty, SlotType.NodeOut);
        AddSlot((int)NodeSlotId.VarKickUser, "KickUser", SlotType.VarIn, typeof(KickUser));
        AddSlot((int)NodeSlotId.VarId, "ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)NodeSlotId.VarUsername, "Username", SlotType.VarOut, typeof(string));
        AddSlot((int)NodeSlotId.VarSlug, "Slug", SlotType.VarOut, typeof(string));
        AddSlot((int)NodeSlotId.VarIdentity, "KickIdentity", SlotType.VarOut, typeof(KickIdentity));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot) {
        var info = new ProcessingInfo { State = LogicState.Ok };
        var val = GetValueFromSlot((int)NodeSlotId.VarKickUser);

        if (val == null || val is not KickUser) {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a KickUser variable.";
            LogManager.Instance.WriteLine(LogVerbosity.Warning, "{0} : {1}", Title, info.ErrorMessage);
        } else {
            KickUser badge = (KickUser)val;
            SetValueInSlot((int)NodeSlotId.VarId, badge.Id);
            SetValueInSlot((int)NodeSlotId.VarUsername, badge.Username);
            SetValueInSlot((int)NodeSlotId.VarSlug, badge.Slug);
            SetValueInSlot((int)NodeSlotId.VarIdentity, badge.Identity);
        }

        ActivateOutputLink(context, (int)NodeSlotId.FlowOut);
        return info;
    }

    protected override SequenceNode CopyImpl() =>
        new KickUserNode();
}
