using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenShock.SDK.CSharp.Utils;

public static class UserAgentUtils {
    // I hate microsoft sometimes
    public static string GetOs() {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "Windows NT";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return "macOS";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return "Linux";
        return "Unknown OS";
    }
    
    public static (string programName, Version programVersion) GetAssemblyInfo() {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null) return UnknownAssemblyInfo;
        var entryAssemblyName = entryAssembly.GetName();
        if (string.IsNullOrWhiteSpace(entryAssemblyName.Name) || entryAssemblyName.Version == null) return UnknownAssemblyInfo;
        
        return (entryAssemblyName.Name, entryAssemblyName.Version);
    }
    
    private static readonly (string, Version) UnknownAssemblyInfo = ("Unknown Assembly", new Version(0, 0, 0));
}