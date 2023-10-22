using System.Xml;
using FlowGraph.Node;

namespace BaseNodes;

public interface ISequenceNodeFactory {
    SequenceNode? CreateNode(XmlNode node);
}