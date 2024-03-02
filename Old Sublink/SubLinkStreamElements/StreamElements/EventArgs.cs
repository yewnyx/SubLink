using System;

namespace xyz.yewnyx.SubLink.StreamElements;

public sealed class TipEventArgs : EventArgs {
    public string Name { get; set; } = string.Empty;
    public float Amount { get; set; } = 0.0f;
    public int CentAmount { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
    public string UserCurrency { get; set; } = string.Empty;
}
