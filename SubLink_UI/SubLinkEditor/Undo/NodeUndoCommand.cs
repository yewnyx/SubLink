﻿using tech.sublink.SubLinkEditor.UI;
using tech.sublink.SubLinkEditor.Controls.NetworkModel;
using tech.sublink.SubLinkEditor.Controls.NetworkUI;

namespace tech.sublink.SubLinkEditor.Undo;

class CreateNodeUndoCommand : IUndoCommand {
    readonly FlowGraphControlViewModel _flowGraphVm;
    readonly NodeViewModel _nodeVm;

    public CreateNodeUndoCommand(FlowGraphControlViewModel fgvm, NodeViewModel nodeVm) {
        _flowGraphVm = fgvm;
        _nodeVm = nodeVm;
    }

    public void Redo() =>
        _flowGraphVm.AddNode(_nodeVm);

    public void Undo() =>
        _flowGraphVm.DeleteNode(_nodeVm);

    public override string ToString() =>
        $"{_flowGraphVm.Sequence.Name} : Create node {_nodeVm.Title}";
}

class CreateNodesUndoCommand : IUndoCommand {
    readonly FlowGraphControlViewModel _flowGraphVm;
    readonly IEnumerable<NodeViewModel> _nodesVm;
    readonly List<ConnectionInfo> _connectionInfoList = new();

    public CreateNodesUndoCommand(FlowGraphControlViewModel fgv, IEnumerable<NodeViewModel> nodesVm) {
        _flowGraphVm = fgv;
        List<ConnectionViewModel> connections = new();

        foreach (var node in nodesVm) {
            connections.AddRange(node.AttachedConnections);
        }

        CopyConnections(connections);
        _nodesVm = nodesVm;
    }

    private void CopyConnections(IEnumerable<ConnectionViewModel> connections) {
        foreach (ConnectionViewModel c in connections) {
            _connectionInfoList.Add(new() {
                ConnectionVm = null,
                DestConnector = c.DestConnector,
                DestConnectorHotspot = c.DestConnectorHotspot,
                Points = c.Points,
                SourceConnector = c.SourceConnector,
                SourceConnectorHotspot = c.SourceConnectorHotspot
            });
        }
    }

    public void Redo() {
        _flowGraphVm.Network.Nodes.AddRange(_nodesVm);
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++) {
            connList.Add(_connectionInfoList[i].ConnectionVm);
        }

        _flowGraphVm.AddConnections(connList);
    }

    public void Undo() {
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++) {
            ConnectionViewModel copy = new() {
                DestConnector = _connectionInfoList[i].DestConnector,
                DestConnectorHotspot = _connectionInfoList[i].DestConnectorHotspot,
                Points = _connectionInfoList[i].Points,
                SourceConnector = _connectionInfoList[i].SourceConnector,
                SourceConnectorHotspot = _connectionInfoList[i].SourceConnectorHotspot
            };

            connList.Add(copy);

            ConnectionInfo inf = _connectionInfoList[i];
            inf.ConnectionVm = copy;
            _connectionInfoList[i] = inf;
        }

        _flowGraphVm.DeleteConnections(connList);
        _flowGraphVm.DeleteNodes(_nodesVm);
    }

    public override string ToString() =>
        $"Graph[{_flowGraphVm.Sequence.Name}] : Create nodes";
}

class DeleteNodeUndoCommand : IUndoCommand {
    readonly FlowGraphControlViewModel _flowGraphVm;
    readonly NodeViewModel _nodeVm;
    readonly List<ConnectionInfo> _connectionInfoList = new();

    public DeleteNodeUndoCommand(FlowGraphControlViewModel fgv, NodeViewModel nodeVm) {
        _flowGraphVm = fgv;
        _nodeVm = nodeVm;
        CopyConnections(_nodeVm.AttachedConnections);
    }

    private void CopyConnections(IEnumerable<ConnectionViewModel> connections) {
        foreach (ConnectionViewModel c in connections) {
            _connectionInfoList.Add(new() {
                ConnectionVm = null,
                DestConnector = c.DestConnector,
                DestConnectorHotspot = c.DestConnectorHotspot,
                Points = c.Points,
                SourceConnector = c.SourceConnector,
                SourceConnectorHotspot = c.SourceConnectorHotspot
            });
        }
    }

    public void Redo() {
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++) {
            connList.Add(_connectionInfoList[i].ConnectionVm);
        }

        _flowGraphVm.DeleteConnections(connList);
        _flowGraphVm.DeleteNode(_nodeVm);
    }

    public void Undo() {
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++) {
            ConnectionViewModel copy = new() {
                DestConnector = _connectionInfoList[i].DestConnector,
                DestConnectorHotspot = _connectionInfoList[i].DestConnectorHotspot,
                Points = _connectionInfoList[i].Points,
                SourceConnector = _connectionInfoList[i].SourceConnector,
                SourceConnectorHotspot = _connectionInfoList[i].SourceConnectorHotspot
            };

            connList.Add(copy);

            ConnectionInfo inf = _connectionInfoList[i];
            inf.ConnectionVm = copy;
            _connectionInfoList[i] = inf;
        }

        _flowGraphVm.AddNode(_nodeVm);
        _flowGraphVm.AddConnections(connList);
    }

    public override string ToString() =>
        $"{_flowGraphVm.Sequence.Name} : Delete node {_nodeVm.Title}";
}

class DeleteNodesUndoCommand : IUndoCommand {
    readonly FlowGraphControlViewModel _flowGraphVm;
    readonly IEnumerable<NodeViewModel> _nodesVm;
    readonly List<ConnectionInfo> _connectionInfoList = new();

    public DeleteNodesUndoCommand(FlowGraphControlViewModel fgv, IEnumerable<NodeViewModel> nodesVm) {
        _flowGraphVm = fgv;
        List<ConnectionViewModel> connections = new();

        foreach (var node in nodesVm) {
            connections.AddRange(node.AttachedConnections);
        }

        CopyConnections(connections);

        _nodesVm = nodesVm;
    }

    private void CopyConnections(IEnumerable<ConnectionViewModel> connections) {
        foreach (ConnectionViewModel c in connections) {
            _connectionInfoList.Add(new() {
                ConnectionVm = null,
                DestConnector = c.DestConnector,
                DestConnectorHotspot = c.DestConnectorHotspot,
                Points = c.Points,
                SourceConnector = c.SourceConnector,
                SourceConnectorHotspot = c.SourceConnectorHotspot
            });
        }
    }

    public void Redo() {
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++) {
            connList.Add(_connectionInfoList[i].ConnectionVm);
        }

        _flowGraphVm.DeleteConnections(connList);
        _flowGraphVm.Network.Nodes.RemoveRange(_nodesVm);
    }

    public void Undo() {
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++) {
            ConnectionViewModel copy = new() {
                DestConnector = _connectionInfoList[i].DestConnector,
                DestConnectorHotspot = _connectionInfoList[i].DestConnectorHotspot,
                Points = _connectionInfoList[i].Points,
                SourceConnector = _connectionInfoList[i].SourceConnector,
                SourceConnectorHotspot = _connectionInfoList[i].SourceConnectorHotspot
            };

            connList.Add(copy);

            ConnectionInfo inf = _connectionInfoList[i];
            inf.ConnectionVm = copy;
            _connectionInfoList[i] = inf;
        }

        _flowGraphVm.Network.Nodes.AddRange(_nodesVm);
        _flowGraphVm.AddConnections(connList);
    }

    public override string ToString() =>
        $"Graph[{_flowGraphVm.Sequence.Name}] : Delete nodes";
}

class PositionNodeUndoCommand : IUndoCommand {
    internal class NodeDraggingInfo {
        public NodeViewModel Node;
        public double StartX, StartY;
        public double EndX, EndY;
    }

    readonly FlowGraphControlViewModel _flowGraphVm;
    readonly IEnumerable<NodeDraggingInfo> _nodeInfosVm;

    public PositionNodeUndoCommand(FlowGraphControlViewModel fgv, IEnumerable<NodeDraggingInfo> nodeInfosVm) {
        _flowGraphVm = fgv;
        _nodeInfosVm = new List<NodeDraggingInfo>(nodeInfosVm);
    }

    public void Redo() {
        foreach (NodeDraggingInfo info in _nodeInfosVm) {
            info.Node.X = info.EndX;
            info.Node.Y = info.EndY;
        }
    }

    public void Undo() {
        foreach (NodeDraggingInfo info in _nodeInfosVm) {
            info.Node.X = info.StartX;
            info.Node.Y = info.StartY;
        }
    }

    public override string ToString() =>
        $"{_flowGraphVm.Sequence.Name} : Node position changed";
}

class SelectNodesUndoCommand : IUndoCommand {
    readonly NetworkView _view;
    readonly IEnumerable<NodeViewModel> _nodesVm;

    public SelectNodesUndoCommand(NetworkView view, IEnumerable<NodeViewModel> nodesVm) {
        _view = view;
        _nodesVm = new List<NodeViewModel>(nodesVm);
    }

    public void Redo() {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm) {
            _view.SelectedNodes.Add(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public void Undo() {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm) {
            _view.SelectedNodes.Remove(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public override string ToString() => "Node selected";
}

class DeselectNodesUndoCommand : IUndoCommand {
    readonly NetworkView _view;
    readonly IEnumerable<NodeViewModel> _nodesVm;

    public DeselectNodesUndoCommand(NetworkView view, IEnumerable<NodeViewModel> nodesVm) {
        _view = view;
        _nodesVm = new List<NodeViewModel>(nodesVm);
    }

    public void Redo() {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm) {
            _view.SelectedNodes.Remove(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public void Undo() {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm) {
            _view.SelectedNodes.Add(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public override string ToString() => "Node deselected";
}
