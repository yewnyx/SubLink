using Nodify;
using SubLink_UI.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;

namespace SubLink_UI.View;

public static class CompositionTargetEx {
    private static TimeSpan _last = TimeSpan.Zero;
    private static event Action<double>? FrameUpdating;

    public static event Action<double> Rendering {
        add {
            if (FrameUpdating == null)
                CompositionTarget.Rendering += OnRendering;

            FrameUpdating += value;
        }
        remove {
            FrameUpdating -= value;

            if (FrameUpdating == null)
                CompositionTarget.Rendering -= OnRendering;
        }
    }

    private static void OnRendering(object? sender, EventArgs e) {
        RenderingEventArgs args = (RenderingEventArgs)e;
        var renderingTime = args.RenderingTime;

        if (renderingTime == _last)
            return;

        double fps = 1000 / (renderingTime - _last).TotalMilliseconds;
        _last = renderingTime;
        FrameUpdating?.Invoke(fps);
    }
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    private readonly Random _rand = new();

    public MainWindow() {
        InitializeComponent();

        CompositionTargetEx.Rendering += OnRendering;
    }

    private void OnRendering(double fps) {
        FPSText.Text = fps.ToString("0");
    }

    private void BringIntoView_Click(object sender, RoutedEventArgs e) {
        if (DataContext is Playground model) {
            NodifyObservableCollection<ViewModel.Nodes.Node> nodes = model.GraphViewModel.Nodes;
            int index = _rand.Next(nodes.Count);

            if (nodes.Count > index) {
                ViewModel.Nodes.Node node = nodes[index];
                EditorCommands.BringIntoView.Execute(node.Location, Editor.Editor);
            }
        }
    }
}
