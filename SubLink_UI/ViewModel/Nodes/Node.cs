using System.Windows;

namespace SubLink_UI.ViewModel.Nodes;

public abstract class Node : ObservableObject {
    private NodifyEditor _graph = default!;
    public NodifyEditor Graph {
        get => _graph;
        internal set => SetProperty(ref _graph, value);
    }

    private Point _location;
    public Point Location {
        get => _location;
        set => SetProperty(ref _location, value);
    }
}
