using System;

namespace SubLink_UI.ViewModel;

public class BaseSetting<T> : ObservableObject, ISetting {
    public string Name { get; }
    public string? Description { get; }

    private object? _value;

    object? ISetting.Value {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public SettingsType Type { get; }

    public T Value {
        get => (T)((ISetting)this).Value!;
        set => ((ISetting)this).Value = value;
    }

    public BaseSetting(string name, string? description = default)
    {
        Name = name;
        Description = description;
        Type = typeof(T) switch {
            { } t when t == typeof(bool) => SettingsType.Boolean,
            { } t when t == typeof(uint) || t == typeof(double) => SettingsType.Number,
            { } t when t == typeof(PointEditor) => SettingsType.Point,
            { IsEnum: true } => SettingsType.Option,
            _ => throw new InvalidOperationException($"Type {typeof(T).Name} does not have a matching {nameof(SettingsType)}.")
        };
    }
}
