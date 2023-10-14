using SubLink_UI.Model;
using System.Windows;
using System.Windows.Input;

namespace SubLink_UI.ViewModel;

public class Connection : ObservableObject {
    private NodifyEditor _graph = default!;
    public NodifyEditor Graph {
        get => _graph;
        internal set => SetProperty(ref _graph, value);
    }

    private Connector _input = default!;
    public Connector Input {
        get => _input;
        set => SetProperty(ref _input, value);
    }

    private Connector _output = default!;
    public Connector Output {
        get => _output;
        set => SetProperty(ref _output, value);
    }

    public void Split(Point point) =>
        Graph.Schema.SplitConnection(this, point);

    public void Remove() =>
        Graph.Connections.Remove(this);

    public ICommand SplitCommand { get; }
    public ICommand DisconnectCommand { get; }

    public Connection() {
        SplitCommand = new DelegateCommand<Point>(Split);
        DisconnectCommand = new DelegateCommand(Remove);
    }
}
