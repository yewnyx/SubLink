namespace xyz.yewnyx.SubLink.OBS.Services;

public class InputVolume {
    public float Multiplier { get; init; }
    public float DB { get; init; }

    internal InputVolume(float multiplier, float dB) {
        Multiplier = multiplier;
        DB = dB;
    }
}

public class TransitionInfo {
    public string Name { get; init; }
    public string Kind { get; init; }
    public bool IsFixed { get; init; }
    public uint? Duration { get; init; }
    public bool IsConfigurable { get; init; }

    internal TransitionInfo(string name, string kind, bool isFixed, bool isConfigurable, uint? duration) {
        Name = name;
        Kind = kind;
        IsFixed = isFixed;
        Duration = duration;
        IsConfigurable = isConfigurable;
    }
}
