using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SubLink_UI.ViewModel;

public class PlaygroundSettings : ObservableObject {
    public IReadOnlyCollection<ISetting> Settings { get; }

    private PlaygroundSettings() {
        Settings = new ObservableCollection<ISetting>() {
            new ProxySetting<bool>(
                () => Instance.ShowGridLines,
                val => Instance.ShowGridLines = val,
                "Show grid lines:"),
            new ProxySetting<bool>(
                () => Instance.AsyncLoading,
                val => Instance.AsyncLoading = val,
                "Async loading:"),
            new ProxySetting<bool>(
                () => Instance.UseCustomConnectors,
                val => Instance.UseCustomConnectors = val,
                "Custom connectors:"),
        };
    }

    public static PlaygroundSettings Instance { get; } = new();

    private bool _asyncLoading = true;
    public bool AsyncLoading {
        get => _asyncLoading;
        set => SetProperty(ref _asyncLoading, value);
    }

    private bool _showGridLines = true;
    public bool ShowGridLines {
        get => _showGridLines;
        set => SetProperty(ref _showGridLines, value);
    }

    private bool _customConnectors = true;
    public bool UseCustomConnectors {
        get => _customConnectors;
        set => SetProperty(ref _customConnectors, value);
    }
}
