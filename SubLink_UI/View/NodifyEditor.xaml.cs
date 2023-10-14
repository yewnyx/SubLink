using System.Windows.Controls;

namespace SubLink_UI.View;

/// <summary>
/// Interaction logic for NodifyEditor.xaml
/// </summary>
public partial class NodifyEditor : UserControl {
    public Nodify.NodifyEditor EditorInstance => Editor;

    public NodifyEditor() {
        InitializeComponent();
    }
}
