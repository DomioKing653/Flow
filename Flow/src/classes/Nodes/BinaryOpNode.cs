using System.Globalization;
using Flow.classes;

namespace Flow.Nodes;

class BinaryOpNode : Node
{
    public Node left;
    public Node right;
    public Token? opTok;

    public BinaryOpNode(Node left, Token? opTok, Node right)
    {
        this.left = left;
        this.right = right;
        this.opTok = opTok;
    }
    public override string ToString()
    {
        return $"({left} {opTok} {right})";
    }

    public override Output VisitNode()
    {
        Output left1 = left.VisitNode();
        Output right1 = right.VisitNode();

        if (left1 is StrOutput leftOutput && right1 is StrOutput rightOutput)
        {
            float r=float.Parse(rightOutput.Value,CultureInfo.InvariantCulture);
            float l=float.Parse(leftOutput.Value,CultureInfo.InvariantCulture);
            switch (opTok?.Type)
            {
                case TokenType.TtPlus:
                    return new StrOutput(leftOutput.Value + rightOutput.Value);
                case TokenType.TtMinus:
                    return new StrOutput(Convert.ToString(l - r));
                case TokenType.TtMul:
                    return new StrOutput(Convert.ToString(l * r));
                case TokenType.TtDiv:
                    return r != 0
                        ? new StrOutput(Convert.ToString(l / r))
                        : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {opTok?.Type}");
            }
        }

        throw new OutputError($"VisitNode {GetType()} not implemented");
    }
}