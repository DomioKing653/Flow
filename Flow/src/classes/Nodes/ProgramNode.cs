using Flow.classes;

namespace Flow.Nodes;

public class ProgramNode : Node
{
    public List<Node> programNodes { get; } = new List<Node>();

    public override Output VisitNode()
    {
        foreach (var node in programNodes)
        {
            node.VisitNode();
        }

        return null;
    }
}