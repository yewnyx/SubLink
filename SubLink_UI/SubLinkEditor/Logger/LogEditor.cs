using System.Collections.ObjectModel;
using System.Windows;
using FlowGraph.Logger;

namespace tech.sublink.SubLinkEditor.Logger;

/// <summary>
/// 
/// </summary>
internal class LogEntry : PropertyChangedBase {
    public DateTime DateTime { get; set; }
    public string Severity { get; set; }
    public string Message { get; set; }
}

/// <summary>
/// 
/// </summary>
internal class CollapsibleLogEntry : LogEntry {
    public List<LogEntry> Contents { get; set; }
}

/// <summary>
/// 
/// </summary>
internal class LogEditor : ILog {
    /// <summary>
    /// 
    /// </summary>
    public static ObservableCollection<LogEntry> LogEntries { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public LogEditor() {
        LogEntries ??= new();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Close() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="verbose"></param>
    /// <param name="msg"></param>
    public void Write(LogVerbosity verbose, string msg) {
        if (Application.Current.Dispatcher.CheckAccess())
            LogEntries.Add(new() {
                Severity = "[" + Enum.GetName(typeof(LogVerbosity), verbose) + "]",
                DateTime = DateTime.Now,
                Message = msg,
            });
        else
            Application.Current.Dispatcher.BeginInvoke(new Action(() => Write(verbose, msg)));
    }
}
