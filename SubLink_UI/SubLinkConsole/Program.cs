using System.Xml;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node.StandardEventNode;
using FlowGraph.Process;

namespace tech.sublink.SubLinkConsole;

class Program {
    const string sequenceName = "appelsap";
    readonly static string sequenceFile = Path.GetFullPath($"../../../../{sequenceName}.xml");
    static bool _starting = true;

    static void Main(string[] args) {
        try {
            LogManager.Instance.AddLogger(new ConsoleLogger());
            LogManager.Instance.Verbosity = LogVerbosity.Trace;
            ProcessLauncher.Instance.PropertyChanged += ProcessLauncher_PropertyChanged;
            ProcessLauncher.Instance.StartLoop();

            XmlDocument xmlDoc = new();
            xmlDoc.Load(sequenceFile);
            XmlNode? rootNode = xmlDoc.SelectSingleNode("SubLinkEditor");

            if (rootNode == null || rootNode.Attributes == null) {
                LogManager.Instance.WriteLine(LogVerbosity.Error, "Invalid XML document");
                return;
            }

            NamedVariableManager.Instance.Load(rootNode);
            GraphDataManager.Instance.Load(rootNode);

            LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully loaded", sequenceFile);

            Sequence seq = GraphDataManager.Instance.GraphList.First(g => g.Name.Equals(sequenceName));
            ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventNodeTestStarted), 0, "test");

            while (_starting || ProcessLauncher.Instance.State == SequenceState.Running) {
                Thread.Sleep(25);
            }

            ProcessLauncher.Instance.StopLoop();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine("Press any keys...");
        Console.ReadKey();
    }

    private static void ProcessLauncher_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals("State") && ProcessLauncher.Instance.State == SequenceState.Running)
            _starting = false;
    }
}
