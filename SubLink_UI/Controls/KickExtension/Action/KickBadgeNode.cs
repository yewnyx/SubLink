using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node;
using FlowGraph.Process;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Action;

[Name("KickBadge"), Category("Objects/Kick")]
public class KickBadgeNode : ActionNode {
    private enum NodeSlotId {
        FlowIn,
        FlowOut,
        VarKickBadge,
        VarType,
        VarText,
        VarCount
    }

    public override string? Title => "KickBadge";

    public KickBadgeNode() { }
    public KickBadgeNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();
        AddSlot((int)NodeSlotId.FlowIn, string.Empty, SlotType.NodeIn);
        AddSlot((int)NodeSlotId.FlowOut, string.Empty, SlotType.NodeOut);
        AddSlot((int)NodeSlotId.VarKickBadge, "KickBadge", SlotType.VarIn, typeof(KickBadge));
        AddSlot((int)NodeSlotId.VarType, "Type", SlotType.VarOut, typeof(string));
        AddSlot((int)NodeSlotId.VarText, "Text", SlotType.VarOut, typeof(string));
        AddSlot((int)NodeSlotId.VarCount, "Count", SlotType.VarOut, typeof(uint));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot) {
        var info = new ProcessingInfo { State = LogicState.Ok };
        var val = GetValueFromSlot((int)NodeSlotId.VarKickBadge);

        if (val == null || val is not KickBadge) {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a KickBadge variable.";
            LogManager.Instance.WriteLine(LogVerbosity.Warning, "{0} : {1}", Title, info.ErrorMessage);
        } else {
            KickBadge badge = (KickBadge)val;
            SetValueInSlot((int)NodeSlotId.VarType, badge.Type);
            SetValueInSlot((int)NodeSlotId.VarText, badge.Text);
            SetValueInSlot((int)NodeSlotId.VarCount, badge.Count);
        }

        ActivateOutputLink(context, (int)NodeSlotId.FlowOut);
        return info;
    }

    protected override SequenceNode CopyImpl() =>
        new KickBadgeNode();
}
