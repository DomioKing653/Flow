namespace Flow;

public class Variable : Node
{
    public double Value { get; }
    public string Identifier { get; }

    public Variable(string identifier, double value)
    {
        Value = value;
        Identifier = identifier;
    }
}
public class Interpreter
{
    private readonly List<Variable> _variables = new List<Variable>();
    public double Interpret(Node node)
    {
        if (node is NumberNode numNode)
        {
            return VisitNumber(numNode);
        }
        if (node is BinaryOpNode binNode)
        {
            return VisitBinaryOp(binNode);
        }
        if (node is VariableNode varNode)
        {
            CreateVar(varNode);
        }
        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");    
       
    }
    void CreateVar(Node node)
    {
        if (node is VariableNode varNode)
        {
            _variables.Add(new Variable(varNode.Identifier.Value,Interpret(varNode.Value)));    
        }
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
        double left = Interpret(node.Left);
        double right = Interpret(node.Right);

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