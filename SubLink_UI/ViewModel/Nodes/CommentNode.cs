using System.Windows;

namespace SubLink_UI.ViewModel.Nodes;

public class CommentNode : Node {
    private string? _title;
    public string? Title {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private Size _size;
    public Size Size {
        get => _size;
        set => SetProperty(ref _size, value);
    }
}
