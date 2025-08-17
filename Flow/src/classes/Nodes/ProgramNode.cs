using Flow.classes;

namespace Flow.Nodes;

public class ProgramNode : Node
{
    public List<Node> ProgramNodes { get; } = new List<Node>();

    public override Output VisitNode()
    {
        foreach (var node in ProgramNodes)
        {
            node.VisitNode();
        }

        return null;
    }
}