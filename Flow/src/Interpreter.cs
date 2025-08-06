namespace Flow;

public class Interpreter
{

    public double Visit(Node node)
    {
        if (node is NumberNode numNode)
        {
            VisitNumber(numNode);
        }
        else if (node is BinaryOpNode binNode)
        {
            return VisitBinaryOp(binNode);
        }
        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");
    }

    double VisitNumber(Node node)
    {
        if (node is NumberNode numberNode)
        {
            return Convert.ToDouble(numberNode.Token?.Value);
        }
        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");
    }

    double VisitBinaryOp(BinaryOpNode node)
    {
        double left = Visit(node.Left);
        double right = Visit(node.Right);

        switch (node.Op?.Type)
        {
            case TokenType.TtPlus:
                return left + right;
            case TokenType.TtMinus:
                return left - right;
            case TokenType.TtMul:
                return left * right;
            case TokenType.TtDiv:
                return right != 0 ? left / right : throw new Exception("Division by zero");
            default:
                throw new Exception($"Unknown operator: {node.Op?.Type}");
        }
    }
}