using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node;
using FlowGraph.Process;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Action;

[Name("KickIdentity"), Category("Objects/Kick")]
public class KickIdentityNode : ActionNode {
    private enum NodeSlotId {
        FlowIn,
        FlowOut,
        VarKickIdentity,
        VarColor,
        VarBadges
    }

    public override string? Title => "KickIdentity";

    public KickIdentityNode() { }
    public KickIdentityNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();
        AddSlot((int)NodeSlotId.FlowIn, string.Empty, SlotType.NodeIn);
        AddSlot((int)NodeSlotId.FlowOut, string.Empty, SlotType.NodeOut);
        AddSlot((int)NodeSlotId.VarKickIdentity, "KickIdentity", SlotType.VarIn, typeof(KickIdentity));
        AddSlot((int)NodeSlotId.VarColor, "Color", SlotType.VarOut, typeof(string));
        AddSlot((int)NodeSlotId.VarBadges, "KickBadge Array", SlotType.VarOut, typeof(KickBadge[]));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot) {
        var info = new ProcessingInfo { State = LogicState.Ok };
        var val = GetValueFromSlot((int)NodeSlotId.VarKickIdentity);

        if (val == null || val is not KickIdentity) {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a KickIdentity variable.";
            LogManager.Instance.WriteLine(LogVerbosity.Warning, "{0} : {1}", Title, info.ErrorMessage);
        } else {
            KickIdentity badge = (KickIdentity)val;
            SetValueInSlot((int)NodeSlotId.VarColor, badge.Color);
            SetValueInSlot((int)NodeSlotId.VarBadges, badge.Badges);
        }

        ActivateOutputLink(context, (int)NodeSlotId.FlowOut);
        return info;
    }

    protected override SequenceNode CopyImpl() =>
        new KickIdentityNode();
}
