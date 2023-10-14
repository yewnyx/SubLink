using SubLink_UI.ViewModel.Nodes;
using System.Linq;
using System.Windows;

namespace SubLink_UI.ViewModel;

public enum ConnectorFlow {
    Input,
    Output
}

public enum ConnectorShape {
    Circle,
    Triangle,
    Square,
}

public class Connector : ObservableObject {
    private string? _title;
    public string? Title {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private bool _isConnected;
    public bool IsConnected {
        get => _isConnected;
        set => SetProperty(ref _isConnected, value);
    }

    private Point _anchor;
    public Point Anchor {
        get => _anchor;
        set => SetProperty(ref _anchor, value);
    }

    private Node _node = default!;
    public Node Node {
        get => _node;
        internal set {
            if (SetProperty(ref _node, value))
                OnNodeChanged();
        }
    }

    private ConnectorShape _shape;
    public ConnectorShape Shape {
        get => _shape;
        set => SetProperty(ref _shape, value);
    }

    public ConnectorFlow Flow { get; private set; }

    public int MaxConnections { get; set; } = 2;

    public NodifyObservableCollection<Connection> Connections { get; } = new();

    public Connector() {
        Connections.WhenAdded(c => {
            c.Input.IsConnected = true;
            c.Output.IsConnected = true;
        })
        .WhenRemoved(c => {
            if (c.Input.Connections.Count == 0)
                c.Input.IsConnected = false;

            if (c.Output.Connections.Count == 0)
                c.Output.IsConnected = false;
        });
    }

    protected virtual void OnNodeChanged() {
        if (Node is FlowNode flow)
            Flow = flow.Input.Contains(this) ? ConnectorFlow.Input : ConnectorFlow.Output;
        else if (Node is KnotNode knot)
            Flow = knot.Flow;
    }

    public bool IsConnectedTo(Connector con) =>
        Connections.Any(c => c.Input == con || c.Output == con);

    public virtual bool AllowsNewConnections() =>
        Connections.Count < MaxConnections;

    public void Disconnect() =>
        Node.Graph.Schema.DisconnectConnector(this);
}
