using Flow.classes.Output;

namespace Flow.classes.Nodes;

public class BoolNode(BooleanType type):Node
{
    public BooleanType Type { get; set; } = type;
    public override Output.Output VisitNode()
    {
        if (Type == BooleanType.True)
            return new ValueOutput(true);
        if (Type == BooleanType.False)
            return new ValueOutput(false);
        throw new Exception("Invalid type lol");
    }
}