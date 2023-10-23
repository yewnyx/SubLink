namespace FlowGraph.Plugin;

public interface IPlugin {
    Task LoadAsync();
    Task UnloadAsync();
}
