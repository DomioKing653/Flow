namespace Flow.classes.Nodes;

public class StringNode:Node
{
    string _stringText;
    public override Output VisitNode()
    {
        return new StrOutput(_stringText);
    }
}