using SubLink_UI.ViewModel.Nodes;

namespace SubLink_UI.ViewModel;

public class PendingConnection : ObservableObject {
    private NodifyEditor _graph = default!;
    public NodifyEditor Graph {
        get => _graph;
        internal set => SetProperty(ref _graph, value);
    }

    private Connector? _source;
    public Connector? Source {
        get => _source;
        set => SetProperty(ref _source, value);
    }

    private object? _previewTarget;
    public object? PreviewTarget {
        get => _previewTarget;
        set {
            if (SetProperty(ref _previewTarget, value))
                OnPreviewTargetChanged();
        }
    }

    private string _previewText = "Drop on connector";
    public string PreviewText {
        get => _previewText;
        set => SetProperty(ref _previewText, value);
    }

    protected virtual void OnPreviewTargetChanged() {
        bool canConnect = PreviewTarget != null && Graph.Schema.CanAddConnection(Source!, PreviewTarget);
        PreviewText = PreviewTarget switch {
            Connector con when con == Source => $"Can't connect to self",
            Connector con => $"{(canConnect ? "Connect" : "Can't connect")} to {con.Title ?? "pin"}",
            FlowNode flow => $"{(canConnect ? "Connect" : "Can't connect")} to {flow.Title ?? "node"}",
            _ => $"Drop on connector"
        };
    }
}
