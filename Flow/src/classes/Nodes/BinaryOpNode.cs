using System.Globalization;
using Flow.classes;

namespace Flow.Nodes;

class BinaryOpNode : Node
{
    public readonly Node Left;
    public readonly Node Right;
    public readonly Token? OpTok;
    public BinaryOpNode(Node left, Token? opTok, Node right)
    {
        this.Left = left;
        this.Right = right;
        this.OpTok = opTok;
    }
    public override string ToString()
    {
        return $"({Left} {OpTok} {Right})";
    }

    public override Output VisitNode()
    {
        Output left1 = Left.VisitNode();
        Output right1 = Right.VisitNode();
        if (left1 is ValueOutput leftOutput && right1 is ValueOutput rightOutput)
        {
            float r = float.Parse(rightOutput.Value);
            float l = float.Parse(leftOutput.Value);
            switch (OpTok?.Type)
            {
                case TokenType.Plus:
                    return new ValueOutput(Convert.ToString(l + r));
                case TokenType.Minus:
                    return new ValueOutput(Convert.ToString(l - r));
                case TokenType.Multiply:
                    return new ValueOutput(Convert.ToString(l * r));
                case TokenType.Divide:
                    return r != 0
                        ? new ValueOutput(Convert.ToString(l / r))
                        : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {OpTok?.Type}");
            }
        }
        throw new OutputError($"VisitNode {GetType()} not implemented");
    }
}