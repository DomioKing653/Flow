using Flow.classes;
using Flow.classes.Nodes;
using Flow.classes.Output;

namespace Flow.Nodes;

public class ProgramNode : ProgramListNode
{
    public override List<Node> Nodes { get; } = new List<Node>();

    public override Output? VisitNode()
    {
        foreach (var node in Nodes)
        {
            node.VisitNode();
        }
        return null;
    }
}