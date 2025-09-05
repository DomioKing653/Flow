using Flow.classes.Output;

namespace Flow.classes.Nodes.LogicOperators;

public class GreaterThanNode(Token? left, Token? right) : Node
{
    private readonly float? _leftToken = float.Parse(left?.Value ?? string.Empty);
    private readonly float? _rightToken = float.Parse(right?.Value ?? string.Empty);

    public override Output.Output? VisitNode()
    {
        if(_leftToken>_rightToken)
        {
            return new ValueOutput(BooleanType.True);
        }
        else
        {
            return new ValueOutput(BooleanType.False);
        }
        throw new Exception("How is this possible lol");
    }
}