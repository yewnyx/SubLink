using System.Windows;

namespace tech.sublink.SubLinkEditor.Controls.AdornedControl;

internal class AdornerEventArgs : RoutedEventArgs {
    public AdornerEventArgs(RoutedEvent routedEvent, object source, FrameworkElement adorner) : base(routedEvent, source) {
        Adorner = adorner;
    }

    public FrameworkElement Adorner { get; }
}

internal delegate void AdornerEventHandler(object sender, AdornerEventArgs e);
