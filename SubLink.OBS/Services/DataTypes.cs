namespace xyz.yewnyx.SubLink.OBS.Services;

public class InputVolume(float multiplier, float dB) {
    public float Multiplier { get; init; } = multiplier;
    public float DB { get; init; } = dB;
}

public class TransitionInfo(string name, string kind, bool isFixed, bool isConfigurable, uint? duration) {
    public string Name { get; init; } = name;
    public string Kind { get; init; } = kind;
    public bool IsFixed { get; init; } = isFixed;
    public uint? Duration { get; init; } = duration;
    public bool IsConfigurable { get; init; } = isConfigurable;
}
