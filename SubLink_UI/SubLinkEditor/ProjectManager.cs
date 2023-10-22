using System.Xml;
using FlowGraph;
using FlowGraph.Logger;
using tech.sublink.SubLinkEditor.FlowGraphs;

namespace tech.sublink.SubLinkEditor;

internal static class ProjectManager {
    public static void Clear() {
        NamedVariableManager.Instance.Clear();
        GraphDataManager.Instance.Clear();
        FlowGraphManager.Instance.Clear();
        MainWindow.Instance.DetailsControl.DataContext = null;
    }

    public static bool OpenFile(string fileName) {
        Clear();

        try {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(fileName);
            XmlNode? rootNode = xmlDoc.SelectSingleNode("SubLinkEditor");

            if (rootNode == null || rootNode.Attributes == null) {
                LogManager.Instance.WriteLine(LogVerbosity.Error, "Invalid XML document");
                return false;
            }

            NamedVariableManager.Instance.Load(rootNode);
            FlowGraphManager.Instance.Load(rootNode); // GraphDataManager.Instance.Load(rootNode) done inside

            LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully loaded", fileName);
        } catch (System.Exception ex) {
            LogManager.Instance.WriteException(ex);
            return false;
        }

        return true;
    }

    public static bool SaveFile(string fileName) {
        const int version = 1;

        try {
            XmlDocument xmlDoc = new();
            XmlNode rootNode = xmlDoc.AddRootNode("SubLinkEditor");
            rootNode.AddAttribute("version", version.ToString());

            FlowGraphManager.Instance.Save(rootNode);
            NamedVariableManager.Instance.Save(rootNode);

            xmlDoc.Save(fileName);

            LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully saved", fileName);
        } catch (System.Exception ex) {
            LogManager.Instance.WriteException(ex);
            return false;
        }

        return true;
    }
}
