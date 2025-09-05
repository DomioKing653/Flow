using Flow.classes.Output;

namespace Flow.classes.Nodes;

public class BoolNode(BooleanType type):Node
{
    private BooleanType Type { get; set; } = type;
    public override Output.Output? VisitNode()
    {
        if (Type == BooleanType.True)
            return new ValueOutput(BooleanType.True);
        if (Type == BooleanType.False)
            return new ValueOutput(BooleanType.False);
        throw new Exception("Invalid type lol");
    }
}