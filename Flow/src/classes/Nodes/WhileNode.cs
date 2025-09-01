namespace Flow.classes.Nodes;

public class WhileNode(List<Node>nodes,Node expression):Node
{
    private readonly List<Node> nodes = nodes;
    public override Output.Output VisitNode()
    {
        foreach (var node in nodes)
        {
            node.VisitNode();
        }
        return new Output.Output();
    }
}