using System;

namespace SubLink_UI.ViewModel;

public class ProxySetting<T> : BaseSetting<T> {
    private readonly Func<T> _getter;
    private readonly Action<T> _setter;

    public ProxySetting(Func<T> getter, Action<T> setter, string name, string? description = default)
        : base(name, description) {
        _getter = getter;
        _setter = setter;
    }

    public new T Value {
        get => _getter();
        set => _setter(value);
    }
}
