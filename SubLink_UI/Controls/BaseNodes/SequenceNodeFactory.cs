using System.Xml;
using BaseNodes.Action;
using BaseNodes.Event;
using BaseNodes.Variable;
using FlowGraph.Node;

namespace BaseNodes;

public class SequenceNodeFactory : ISequenceNodeFactory {
    public SequenceNode? CreateNode(XmlNode node) {
        var nodeAttribute = node.Attributes?["type"];

        if (nodeAttribute == null)
            return null;

        var typeName = nodeAttribute.Value;

        if (typeName.Equals(typeof(AdditionNodeInt).FullName))
            return new AdditionNodeInt(node);

        if (typeName.Equals(typeof(LogMessageNode).FullName))
            return new LogMessageNode(node);

        if (typeName.Equals(typeof(EventTaskStartedNode).FullName))
            return new EventTaskStartedNode(node);

        if (typeName.Equals(typeof(EventMessageReceivedNode).FullName))
            return new EventMessageReceivedNode(node);

        if (typeName.Equals(typeof(VariableNodeInt).FullName))
            return new VariableNodeInt(node);

        throw new ArgumentException($"The type {typeName} is not supported");
    }
}
