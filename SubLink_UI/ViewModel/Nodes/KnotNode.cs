namespace SubLink_UI.ViewModel.Nodes;

public class KnotNode : Node {
    private Connector _connector = default!;
    public Connector Connector {
        get => _connector;
        set {
            if (SetProperty(ref _connector, value))
                _connector.Node = this;
        }
    }

    public ConnectorFlow Flow { get; set; }
}
