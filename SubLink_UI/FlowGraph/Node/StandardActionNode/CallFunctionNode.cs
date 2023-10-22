using System;
using System.ComponentModel;
using System.Xml;
using FlowGraph.Node.StandardEventNode;
using FlowGraph.Process;

namespace FlowGraph.Node.StandardActionNode;

[Visible(false)]
public class CallFunctionNode : ActionNode {
    public enum NodeSlotId {
        In,
        Out,
        InputStart,
        OutputStart = 1073741823 // int.MaxValue / 2
    }

    private int _functionId = -1; // used when the node is loaded, in order to retrieve the function
    private SequenceFunction _function;

    public override string Title => GetFunction().Name + " function";

    public CallFunctionNode(SequenceFunction function) {
        _function = function;
        SetFunction(function);
    }

    public CallFunctionNode(XmlNode node) : base(node) { }

    private void OnFunctionSlotChanged(object sender, FunctionSlotChangedEventArg e) {
        if (e.Type == FunctionSlotChangedType.Added) {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                AddFunctionSlot((int)NodeSlotId.InputStart + e.FunctionSlot.Id, SlotType.VarIn, e.FunctionSlot);
            else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
                AddFunctionSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, SlotType.VarOut, e.FunctionSlot);
        } else if (e.Type == FunctionSlotChangedType.Removed) {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                RemoveSlotById((int)NodeSlotId.InputStart + e.FunctionSlot.Id);
            else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
                RemoveSlotById((int)NodeSlotId.OutputStart + e.FunctionSlot.Id);
        }

        OnPropertyChanged("Slots");
    }

    private void UpdateNodeSlot() {
        GetFunction();

        foreach (var slot in GetFunction().Inputs) {
            AddFunctionSlot((int)NodeSlotId.InputStart + slot.Id, SlotType.VarIn, slot);
        }

        foreach (var slot in GetFunction().Outputs) {
            AddFunctionSlot((int)NodeSlotId.OutputStart + slot.Id, SlotType.VarOut, slot);
        }

        OnPropertyChanged("Slots");
    }

    private SequenceFunction GetFunction() {
        if (_function == null && _functionId != -1) {
            _function = GraphDataManager.Instance.GetFunctionById(_functionId);
            SetFunction(_function);
        }

        return _function;
    }

    private void SetFunction(SequenceFunction func) {
        _function.PropertyChanged += OnFunctionPropertyChanged!;
        _function.FunctionSlotChanged += OnFunctionSlotChanged!;
        UpdateNodeSlot();
    }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "In", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "Out", SlotType.NodeOut);
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot) {
        var info = new ProcessingInfo { State = LogicState.Ok };
        var func = GetFunction();
        func.OnReturn += OnReturn;
        context.RegisterNextSequence(func, typeof(OnEnterFunctionEvent), null);
        
        return info;
    }

    private void OnReturn(ProcessingContext context) {
        GetFunction().OnReturn -= OnReturn;
        ActivateOutputLink(context, (int)NodeSlotId.Out);
    }

    protected override SequenceNode CopyImpl() =>
        new CallFunctionNode(GetFunction());

    protected override void Load(XmlNode node) {
        base.Load(node);
        _functionId = int.Parse(node.Attributes["functionID"].Value);
    }

    internal override void ResolveLinks(XmlNode connectionListNode, SequenceBase sequence) {
        GetFunction();
        base.ResolveLinks(connectionListNode, sequence);
    }

    public override void Save(XmlNode node) {
        base.Save(node);
        node.AddAttribute("functionID", GetFunction().Id.ToString());
    }

    private void OnFunctionPropertyChanged(object sender, PropertyChangedEventArgs e) {
        switch (e.PropertyName) {
            case "Name": {
                OnPropertyChanged("Title");
                break;
            }
        }
    }
}
