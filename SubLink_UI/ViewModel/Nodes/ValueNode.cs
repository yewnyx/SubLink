namespace SubLink_UI.ViewModel.Nodes;

public class ValueNode : Node {
    private string? _title;
    public string? Title {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public NodifyObservableCollection<Connector> Output { get; } = new();
}
