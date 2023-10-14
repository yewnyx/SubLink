using SubLink_UI.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SubLink_UI.View;

/// <summary>
/// Interaction logic for Settings.xaml
/// </summary>
public partial class Settings : UserControl {
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(IEnumerable<ISetting>), typeof(Settings));

    public IEnumerable<ISetting> Items {
        get => (IEnumerable<ISetting>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public Settings() {
        InitializeComponent();
    }
}
