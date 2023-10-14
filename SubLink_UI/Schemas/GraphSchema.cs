using SubLink_UI.ViewModel;
using SubLink_UI.ViewModel.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SubLink_UI.Schemas;

public class GraphSchema {
    #region Add Connection

    public bool CanAddConnection(Connector source, object target) {
        if (target is Connector con) {
            return source != con
                && source.Node != con.Node
                && source.Node.Graph == con.Node.Graph
                && source.Shape == con.Shape
                && source.AllowsNewConnections()
                && con.AllowsNewConnections()
                && (source.Flow != con.Flow || con.Node is KnotNode)
                && !source.IsConnectedTo(con);
        } else if (source.AllowsNewConnections() && target is FlowNode node) {
            var allConnectors = source.Flow == ConnectorFlow.Input ? node.Output : node.Input;
            return allConnectors.Any(c => c.AllowsNewConnections());
        }

        return false;
    }

    public bool TryAddConnection(Connector source, object? target) {
        if (target != null && CanAddConnection(source, target)) {
            if (target is Connector connector) {
                AddConnection(source, connector);
                return true;
            } else if (target is FlowNode node) {
                AddConnection(source, node);
                return true;
            }
        }

        return false;
    }

    private void AddConnection(Connector source, Connector target) {
        var sourceIsInput = source.Flow == ConnectorFlow.Input;

        source.Node.Graph.Connections.Add(new Connection {
            Input = sourceIsInput ? source : target,
            Output = sourceIsInput ? target : source
        });
    }

    private void AddConnection(Connector source, FlowNode target) {
        var allConnectors = source.Flow == ConnectorFlow.Input ? target.Output : target.Input;
        var connector = allConnectors.First(c => c.AllowsNewConnections());

        AddConnection(source, connector);
    }

    #endregion

    public void DisconnectConnector(Connector connector) {
        var graph = connector.Node.Graph;
        var connections = connector.Connections.ToList();
        connections.ForEach(c => graph.Connections.Remove(c));
    }

    public void SplitConnection(Connection connection, Point location) {
        var connector = connection.Output;

        var knot = new KnotNode {
            Location = location,
            Flow = connector.Flow,
            Connector = new Connector {
                MaxConnections = connection.Output.MaxConnections + connection.Input.MaxConnections,
                Shape = connection.Input.Shape
            }
        };
        connection.Graph.Nodes.Add(knot);

        AddConnection(connector, knot.Connector);
        AddConnection(knot.Connector, connection.Input);

        connection.Remove();
    }

    public void AddCommentAroundNodes(IList<Node> nodes, string? text = default) {
        var rect = nodes.GetBoundingBox(50);
        var comment = new CommentNode {
            Location = rect.Location,
            Size = rect.Size,
            Title = text ?? "New comment"
        };

        nodes[0].Graph.Nodes.Add(comment);
    }
}
