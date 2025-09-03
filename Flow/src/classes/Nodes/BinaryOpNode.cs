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
            float r = float.Parse(rightOutput.Value);
            float l = float.Parse(leftOutput.Value);
            switch (_opTok?.Type)
            {
                case TokenType.Plus:
                    return new ValueOutput(Convert.ToString(l + r, CultureInfo.CurrentCulture));
                case TokenType.Minus:
                    return new ValueOutput(Convert.ToString(l - r, CultureInfo.CurrentCulture));
                case TokenType.Multiply:
                    return new ValueOutput(Convert.ToString(l * r, CultureInfo.CurrentCulture));
                case TokenType.Divide:
                    return r != 0
                        ? new ValueOutput(Convert.ToString(l / r, CultureInfo.CurrentCulture))
                        : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {_opTok?.Type}");
            }
        }
        throw new OutputError($"VisitNode {GetType()} not implemented");
    }
}