using System.Globalization;
using Flow.classes;
using Flow.classes.Output;

namespace Flow.Nodes;

class BinaryOpNode : Node
{
    private readonly Node _left;
    private readonly Node _right;
    private readonly Token? _opTok;

    public BinaryOpNode(Node left, Token? opTok, Node right)
    {
        _left = left;
        _right = right;
        _opTok = opTok;
    }

    public override string ToString()
    {
        return $"({_left} {_opTok} {_right})";
    }

    public override Output? VisitNode()
    {
        Output? left1 = _left.VisitNode();
        Output? right1 = _right.VisitNode();
        if (left1 is ValueOutput leftOutput && right1 is ValueOutput rightOutput)
        {
            float r;
            float l;
            if (leftOutput is not null && rightOutput is not null)
            {
                switch (_opTok?.Type)
                {
                    case TokenType.Plus:
                        if (leftOutput.Value is string strLeft && rightOutput.Value is string strRight)
                        {
                            return new ValueOutput(Convert.ToString(strLeft + strRight, CultureInfo.InvariantCulture));
                        }

                        r = float.Parse(rightOutput.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                        l = float.Parse(leftOutput.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                        return new ValueOutput(Convert.ToString(l + r, CultureInfo.CurrentCulture));
                    case TokenType.Minus:
                        l = float.Parse(leftOutput.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                        r = float.Parse(rightOutput.Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                        return new ValueOutput(Convert.ToString(l - r, CultureInfo.CurrentCulture));
                    case TokenType.Multiply:
                        r = float.Parse(rightOutput.Value, CultureInfo.CurrentCulture);
                        l = float.Parse(leftOutput.Value, CultureInfo.CurrentCulture);
                        return new ValueOutput(Convert.ToString(l * r, CultureInfo.CurrentCulture));
                    case TokenType.Divide:
                        r = float.Parse(rightOutput.Value, CultureInfo.CurrentCulture);
                        l = float.Parse(leftOutput.Value, CultureInfo.CurrentCulture);
                        return r != 0
                            ? new ValueOutput(Convert.ToString(l / r, CultureInfo.CurrentCulture))
                            : throw new Exception("Division by zero");
                    default:
                        throw new Exception($"Unknown operator: {_opTok?.Type}");
                }
            }
        }

        throw new OutputError($"VisitNode {GetType()} not implemented");
    }
}