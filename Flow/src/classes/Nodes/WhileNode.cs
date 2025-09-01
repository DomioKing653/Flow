namespace Flow.classes.Nodes;

public class WhileNode(List<Node>nodes):Node
{
    public List<Node> Nodes = nodes;
    public override Output.Output VisitNode()
    {
        foreach (var node in Nodes)
        {
            node.VisitNode();
        }
        return new Output.Output();
    }
}