using System.Globalization;

namespace Flow;

/*
 * Nodes
 */
public abstract class Node
{
    /*
     * It's kinda useless just for uniting all Node classes
     */
    public abstract Output VisitNode();
    
}
public class ProgramNode : Node
{
    private List<Node> programNodes { get; }=new List<Node>();
    public override Output VisitNode()
    {
        foreach (var node in programNodes)
        {
            node.VisitNode();
        }
        return null;
    }
}
class VariableSetNode(string id, Node value) : Node
{
    public string Identifier { get; } = id;
    public Node Value { get; } = value;
    public override Output VisitNode()
    {
        throw new NotImplementedException();
    }
}

class PrintNode(Node expression) : Node
{
    private Node Expression { get; } = expression;

    public override Output VisitNode()
    {
        var value = Expression.VisitNode();
        Console.WriteLine(value);
        return new Output();
    }
}

public class VariableAccessNode(Token identifier) : Node
{
    private Token Identifier { get; } = identifier;

    public override string ToString()
    {
        return Identifier.Value;
    }

    public override Output VisitNode()
    {
        var variable = VariableManagement.Variables.FirstOrDefault(v => v != null && v.Identifier == Identifier.Value);
        if (variable is null)
        {
            throw new OutputError($"Variable {Identifier.Value} not found");
        }

        return new NumbOutput(variable.Value);
    }
}

public class VariableNode(Token identifier, Node value) : Node
{
    public Token Identifier { get; set; } = identifier;
    public Node Value { get; set; } = value;

    public override string ToString()
    {
        return $"{Identifier.Value} = {Value}";
    }

    public override Output VisitNode()
    {
        return VariableManagement.AddVariable(this);

    }
}
class NumberNode(Token? token) : Node
{
    private Token? Token { get; set; } = token;

    public override string ToString()
    {
        return $"({Token})";
    }

    public override Output VisitNode()
    {
        return new NumbOutput(float.Parse(Token?.Value,CultureInfo.InvariantCulture));
    }
}

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
