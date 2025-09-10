using Flow.classes.Output;

namespace Flow.classes.Nodes.LogicOperators;

public class CompareNode(Node left,TokenType type, Node? right) : Node
{
    private readonly Node? _leftToken = left;
    private readonly Node? _rightToken = right;

    public override Output.Output? VisitNode()
    {
        if (_leftToken.VisitNode() is ValueOutput valueL&&_rightToken.VisitNode() is ValueOutput valueR)
        {
            if (type== TokenType.GreaterThan)
            {
                if (valueL.FloatValue > valueR.FloatValue)
                {
                    return new ValueOutput(BooleanType.True);
                }
                else
                {
                    return new ValueOutput(BooleanType.False);
                }
            }
        }
        throw new Exception("Idk");
    }
}