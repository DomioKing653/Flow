using Flow.classes.Output;
using Flow.Program;

namespace Flow.classes.Nodes;

public class WhileNode(Node expression) : ProgramListNode
{
    public override List<Node> Nodes { get; } = new List<Node>();

    public override Output.Output? VisitNode()
    {
        while (true)
        {
            var expr = expression.VisitNode();
            if (expr is ValueOutput value)
            {
                if (value.BoolValue is not null)
                {
                    if (value.BoolValue == BooleanType.True)
                    {
                        foreach (var node in Nodes)
                        {
                            node.VisitNode();
                            expr = expression.VisitNode();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (value.BoolValue is null)
                    throw new OutputError("Expected boolean value");
            }
            else
            {
                return null;
            }
        }

        return null;
    }
}