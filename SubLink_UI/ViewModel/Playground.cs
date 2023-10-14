using SubLink_UI.Model;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SubLink_UI.ViewModel;

public class Playground : ObservableObject {
    public NodifyEditor GraphViewModel { get; } = new();

    public Playground() {
        ResetCommand = new DelegateCommand(ResetGraph);

        BindingOperations.EnableCollectionSynchronization(GraphViewModel.Nodes, GraphViewModel.Nodes);
        BindingOperations.EnableCollectionSynchronization(GraphViewModel.Connections, GraphViewModel.Connections);
    }

    public ICommand ResetCommand { get; }
    public PlaygroundSettings Settings => PlaygroundSettings.Instance;

    private void ResetGraph() {
        GraphViewModel.Nodes.Clear();
        EditorSettings.Instance.Location = new Point(0, 0);
        EditorSettings.Instance.Zoom = 1.0d;
    }
}
