using System.Xml;

namespace FlowGraph.Node.StandardEventNode;

[Name("Test Trigger Event"), Category("Event")]
public class EventNodeTestStarted : EventNode {
    public override string Title => "Test Trigger Event";

    public EventNodeTestStarted(XmlNode node) : base(node) { }

    public EventNodeTestStarted() { }

    protected override void InitializeSlots() {
        base.InitializeSlots();

        AddSlot(0, "Triggered", SlotType.NodeOut);
        AddSlot(1, "Task name", SlotType.VarOut, typeof(string));
    }

    protected override void TriggeredImpl(object? para) =>
        SetValueInSlot(1, para);

    protected override SequenceNode CopyImpl() =>
        new EventNodeTestStarted();
}
