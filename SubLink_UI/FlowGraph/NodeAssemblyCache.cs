using FlowGraph.Node;
using System.Reflection;

namespace FlowGraph;

public static class NodeAssemblyCache
{
    private static string _dllPath;

    private static readonly List<Assembly> _assemblies = new();
    public static IReadOnlyList<Assembly> Assemblies { get => _assemblies; }

    private static readonly HashSet<string> _assemblyNames = new();
    public static IReadOnlySet<string> AssemblyNames { get => _assemblyNames; }

    public static IEnumerable<Type> Nodes {
        get => Assemblies
            .SelectMany(t => t.GetTypes())
            .Where(t => t.IsClass &&
                t.IsGenericType == false &&
                t.IsInterface == false &&
                t.IsAbstract == false &&
                t.IsSubclassOf(typeof(SequenceNode)));
    }

    static NodeAssemblyCache()
    {
        _dllPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location) ?? ".",
            "Extentions"
        );
        Refresh();
    }

    public static void Refresh()
    {
        Assembly curAsm;
        IEnumerable<Type> nodeTypes;

        _assemblyNames.Clear();
        _assemblies.Clear();

        foreach (var reference in Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Where(
            x => x.Name != null && !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System")
        )) {
            if (!_assemblyNames.Contains(reference.FullName)) {
                var assembly = Assembly.Load(reference);
                _assemblyNames.Add(reference.FullName);
                _assemblies.Add(assembly);
            }
        }

        foreach (var file in Directory.GetFiles(_dllPath, "*.dll")) {
            if (file.Contains("FlowGraph.") || file.Contains("Microsoft.") || file.Contains("System."))
                continue;

            try {
                curAsm = Assembly.LoadFrom(file);

                if (curAsm == null)
                    continue;

                nodeTypes = curAsm.GetTypes().Where(t => t.IsClass &&
                    t.IsGenericType == false &&
                    t.IsInterface == false &&
                    t.IsAbstract == false &&
                    t.IsSubclassOf(typeof(SequenceNode)));

#pragma warning disable CS8604 // Possible null reference argument.
                if (!nodeTypes.Any() || _assemblyNames.Contains(curAsm.FullName))
                    continue;
#pragma warning restore CS8604 // Possible null reference argument.

                _assemblyNames.Add(curAsm.FullName);
                _assemblies.Add(curAsm);
            } catch { }
        }
    }
}
