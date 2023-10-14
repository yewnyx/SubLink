using SubLink_UI.Model;
using SubLink_UI.Schemas;
using SubLink_UI.ViewModel.Nodes;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SubLink_UI.ViewModel;

public class NodifyEditor : ObservableObject {
    public NodifyEditor() {
        Schema = new();

        PendingConnection = new() { Graph = this };

        DeleteSelectionCommand = new DelegateCommand(DeleteSelection, () => SelectedNodes.Count > 0);
        CommentSelectionCommand = new RequeryCommand(() => Schema.AddCommentAroundNodes(SelectedNodes, "New comment"), () => SelectedNodes.Count > 0);
        DisconnectConnectorCommand = new DelegateCommand<Connector>(c => c.Disconnect());
        CreateConnectionCommand = new DelegateCommand<object>(target => Schema.TryAddConnection(PendingConnection.Source!, target), target => PendingConnection.Source != null && target != null);

        Connections.WhenAdded(c => {
            c.Graph = this;
            c.Input.Connections.Add(c);
            c.Output.Connections.Add(c);
        })
        // Called when the collection is cleared
        .WhenRemoved(c => {
            c.Input.Connections.Remove(c);
            c.Output.Connections.Remove(c);
        });

        Nodes.WhenAdded(x => x.Graph = this)
             // Not called when the collection is cleared
             .WhenRemoved(x => {
                 if (x is FlowNode flow)
                     flow.Disconnect();
                 else if (x is KnotNode knot)
                     knot.Connector.Disconnect();
             })
             .WhenCleared(x => Connections.Clear());
    }

    private NodifyObservableCollection<Node> _nodes = new();
    public NodifyObservableCollection<Node> Nodes {
        get => _nodes;
        set => SetProperty(ref _nodes, value);
    }

    private NodifyObservableCollection<Node> _selectedNodes = new();
    public NodifyObservableCollection<Node> SelectedNodes {
        get => _selectedNodes;
        set => SetProperty(ref _selectedNodes, value);
    }

    private NodifyObservableCollection<Connection> _connections = new();
    public NodifyObservableCollection<Connection> Connections {
        get => _connections;
        set => SetProperty(ref _connections, value);
    }

    private Size _viewportSize;
    public Size ViewportSize {
        get => _viewportSize;
        set => SetProperty(ref _viewportSize, value);
    }

    public PendingConnection PendingConnection { get; }
    public GraphSchema Schema { get; }

    public ICommand DeleteSelectionCommand { get; }
    public ICommand DisconnectConnectorCommand { get; }
    public ICommand CreateConnectionCommand { get; }
    public ICommand CommentSelectionCommand { get; }

    private void DeleteSelection() {
        var selected = SelectedNodes.ToList();

        for (int i = 0; i < selected.Count; i++) {
            Nodes.Remove(selected[i]);
        }
    }
}
