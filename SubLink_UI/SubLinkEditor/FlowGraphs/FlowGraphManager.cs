﻿using System.Collections.ObjectModel;
using System.Xml;
using FlowGraph;
using FlowGraph.Logger;
using tech.sublink.SubLinkEditor.UI;

namespace tech.sublink.SubLinkEditor.FlowGraphs;

internal class FlowGraphManager {
    public static FlowGraphManager Instance { get; } = new();

    public ObservableCollection<FlowGraphControlViewModel> FlowGraphList { get; } = new();

    private FlowGraphManager() { }

    public FlowGraphControlViewModel GetViewModelById(int id) {
        foreach (FlowGraphControlViewModel vm in FlowGraphList) {
            if (vm.Id == id)
                return vm;
        }

        return null;
    }

    internal void Clear() =>
        FlowGraphList.Clear();

    internal void Add(FlowGraphControlViewModel viewModel) =>
        FlowGraphList.Add(viewModel);

    internal void Remove(FlowGraphControlViewModel viewModel) =>
        FlowGraphList.Remove(viewModel);

    public static bool IsChanges() => false;

    public void Load(XmlNode node) {
        try {
            GraphDataManager.Instance.Load(node);

            int version = int.Parse(node.SelectSingleNode("GraphList").Attributes["version"].Value);

            foreach (XmlNode graphNode in node.SelectNodes("GraphList/Graph")) {
                FlowGraphList.Add(new FlowGraphControlViewModel(graphNode));
            }
        } catch (System.Exception ex) {
            LogManager.Instance.WriteException(ex);
        }
    }

    public void Save(XmlNode node) {
        // Create all sequence to reflect all changes make (because it is not done in real time)
        foreach (FlowGraphControlViewModel wm in FlowGraphList) {
            wm.CreateSequence();
        }

        GraphDataManager.Instance.Save(node);

        const int version = 1;
        XmlNode list = node.SelectSingleNode("GraphList");
        list.AddAttribute("designerVersion", version.ToString());

        foreach (FlowGraphControlViewModel wm in FlowGraphList) {
            wm.Save(list);
        }
    }
}