using Flow.classes;

namespace Flow.Nodes;

class BinaryOpNode(Node left, Token? opTok, Node right) : Node
{
    public override string ToString()
    {
        return $"({left} {opTok} {right})";
    }

    public override Output VisitNode()
    {
        Output left1 = left.VisitNode();
        Output right1 = left.VisitNode();
        if (left1 is NumbOutput leftOutput && right1 is NumbOutput rightOutput)
        {
            switch (opTok?.Type)
            {
                case TokenType.TtPlus:
                    return new NumbOutput(leftOutput.Value + rightOutput.Value);
                case TokenType.TtMinus:
                    return new NumbOutput(leftOutput.Value - rightOutput.Value);
                case TokenType.TtMul:
                    return new NumbOutput(leftOutput.Value * rightOutput.Value);
                case TokenType.TtDiv:
                    return rightOutput.Value != 0
                        ? new NumbOutput(leftOutput.Value / rightOutput.Value)
                        : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {opTok?.Type}");
            }
        }

        throw new OutputError($"VisitNode {GetType()} not implemented");
    }
    
}
