using System.Xml;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node.StandardEventNode;
using FlowGraph.Process;

namespace tech.sublink.SubLinkConsole;

class Program {
    static void Main(string[] args) {
        try {
            ProcessLauncher.Instance.StartLoop();

            LogManager.Instance.AddLogger(new ConsoleLogger());
            LogManager.Instance.Verbosity = LogVerbosity.Trace;

            XmlDocument xmlDoc = new();
            xmlDoc.Load("../../../../appelsap.xml");

            // TODO : how load NamedVariableList and Sequence
            // if the structure of the xml is not defined in the library ?
            // It is the user who build the xml
            XmlNode rootNode = xmlDoc.FirstChild.NextSibling;
            NamedVariableManager.Instance.Load(rootNode);
            GraphDataManager.Instance.Load(rootNode);

            Sequence seq = new("appelsap");
            seq.Load(xmlDoc.SelectSingleNode("SubLinkEditor/GraphList/Graph[@name='appelsap']"));

            ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventNodeTestStarted), 0, "test");

            while (ProcessLauncher.Instance.State == SequenceState.Running) {
                Thread.Sleep(100);
            }

            ProcessLauncher.Instance.StopLoop();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine("Press any keys...");
        Console.ReadKey();
    }
}
