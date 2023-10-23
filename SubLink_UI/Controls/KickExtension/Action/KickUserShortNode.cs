using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node;
using FlowGraph.Process;
using System.Xml;
using tech.sublink.KickExtension.Kick.Types;

namespace tech.sublink.KickExtension.Action;

[Name("KickUserShort"), Category("Objects/Kick")]
public class KickUserShortNode : ActionNode {
    private enum NodeSlotId {
        FlowIn,
        FlowOut,
        VarKickUser,
        VarId,
        VarUsername,
        VarSlug
    }

    public override string? Title => "KickUserShort";

    public KickUserShortNode() { }
    public KickUserShortNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots() {
        base.InitializeSlots();
        AddSlot((int)NodeSlotId.FlowIn, string.Empty, SlotType.NodeIn);
        AddSlot((int)NodeSlotId.FlowOut, string.Empty, SlotType.NodeOut);
        AddSlot((int)NodeSlotId.VarKickUser, "KickUserShort", SlotType.VarIn, typeof(KickUserShort));
        AddSlot((int)NodeSlotId.VarId, "ID", SlotType.VarOut, typeof(uint));
        AddSlot((int)NodeSlotId.VarUsername, "Username", SlotType.VarOut, typeof(string));
        AddSlot((int)NodeSlotId.VarSlug, "Slug", SlotType.VarOut, typeof(string));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot) {
        var info = new ProcessingInfo { State = LogicState.Ok };
        var val = GetValueFromSlot((int)NodeSlotId.VarKickUser);

        if (val == null || val is not KickUserShort) {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a KickUserShort variable.";
            LogManager.Instance.WriteLine(LogVerbosity.Warning, "{0} : {1}", Title, info.ErrorMessage);
        } else {
            KickUserShort badge = (KickUserShort)val;
            SetValueInSlot((int)NodeSlotId.VarId, badge.Id);
            SetValueInSlot((int)NodeSlotId.VarUsername, badge.Username);
            SetValueInSlot((int)NodeSlotId.VarSlug, badge.Slug);
        }

        ActivateOutputLink(context, (int)NodeSlotId.FlowOut);
        return info;
    }

    protected override SequenceNode CopyImpl() =>
        new KickUserShortNode();
}
