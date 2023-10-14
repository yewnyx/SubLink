namespace SubLink_UI.ViewModel.Nodes;

public class FlowNode : Node {
    private string? _title;
    public string? Title {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public NodifyObservableCollection<FlowConnector> Input { get; } = new();
    public NodifyObservableCollection<FlowConnector> Output { get; } = new();

    public FlowNode() {
        Input.WhenAdded(c => c.Node = this)
             .WhenRemoved(c => c.Disconnect());

        Output.WhenAdded(c => c.Node = this)
             .WhenRemoved(c => c.Disconnect());
    }

    public void Disconnect() {
        Input.Clear();
        Output.Clear();
    }
}
