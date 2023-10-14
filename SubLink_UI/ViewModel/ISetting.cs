namespace SubLink_UI.ViewModel;

public enum SettingsType {
    Boolean,
    Number,
    Option,
    Point
}

public interface ISetting {
    string Name { get; }

    /// <summary>
    /// Represents the content within the tooltip.
    /// </summary>
    string? Description { get; }
    object? Value { get; set; }

    SettingsType Type { get; }
}
