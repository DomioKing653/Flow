using System.Globalization;
using Flow.classes;
using Flow.classes.Output;

namespace Flow.Nodes;

class BinaryOpNode(Node left, Token? opTok, Node right) : Node
{
    public override string ToString()
    {
        return $"({left} {opTok} {right})";
    }

    public override Output? VisitNode()
    {
        Output? left1 = left.VisitNode();
        Output? right1 = right.VisitNode();
        if (left1 is ValueOutput leftOutput && right1 is ValueOutput rightOutput)
        {
            
            float r=(float)rightOutput.FloatValue!;
            float l=(float)leftOutput.FloatValue!;
            string? rs = null;
            string? ls = null;
            if (leftOutput.Value  is not null&& rightOutput.Value is not null)
            {
                rs = rightOutput.Value;
                ls = leftOutput.Value;
                
            }
            
            if (leftOutput is not null && rightOutput is not null)
            {
                switch (opTok?.Type)
                {
                    case TokenType.Plus:
                        if (rs is not null && ls is not null)
                        {
                            return new ValueOutput(Convert.ToString(ls + rs, CultureInfo.InvariantCulture));
                        }

                        return new ValueOutput(l + r);
                    case TokenType.Minus:
                        return new ValueOutput(l - r);
                    case TokenType.Multiply:
                        return new ValueOutput(l * r);
                    case TokenType.Divide:
                        return r != 0
                            ? new ValueOutput(l / r)
                            : throw new Exception("Division by zero");
                    default:
                        throw new Exception($"Unknown operator: {opTok?.Type}");
                }
            }
        }

        throw new OutputError($"VisitNode {GetType()} not implemented");
    }
}