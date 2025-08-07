namespace Flow;


/*
 * Nodes
 */
public class Output
{
        
}

public class NumbOutput(double value) : Output
{
    public readonly double Value = value;
    public override string ToString()
    {
        return $"Solution is {Value}";
    }
}
public class Variable(string identifier, double value) : Output
{
    public double Value { get; } = value;
    public string Identifier { get; } = identifier;

    public override string ToString()
    {
        return $"{Identifier} = {Value}";
    }
}
public class Interpreter
{
    
    private readonly List<Variable> _variables = new List<Variable>();
    public Output Interpret(Node node)
    {
        if (node is NumberNode numNode)
        {
            return VisitNumber(numNode);
        }
        else if (node is BinaryOpNode binNode)
        {
            return VisitBinaryOp(binNode);
        }
        else if (node is VariableNode varNode)
        {
           return CreateVar(varNode);
        }
        else if (node is VariableAccessNode accNode)
        {
            return VisitAccessNode(accNode); // viz níže
        }else if (node is PrintNode printNode)
        {
            var value = Interpret(printNode.Expression);
            Console.WriteLine(value);
        }
        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");    
       
    }
    
    Variable CreateVar(VariableNode varNode)
    {
        Output output = Interpret(varNode.Value);

        if (output is NumbOutput numbOutput)
        {
            if (varNode.Identifier.Value != null)
            {
                var variable = new Variable(varNode.Identifier.Value, numbOutput.Value);
                _variables.Add(variable);
                return variable;
            }
        }
        throw new Exception("Invalid value in variable assignment.");
    }
    NumbOutput VisitNumber(Node node)
    {
        if (node is NumberNode numberNode)
        {
            return new NumbOutput(Convert.ToDouble(numberNode.Token?.Value));
        }

        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");
    }

    Output VisitAccessNode(VariableAccessNode node)
    {
        var variable = _variables.FirstOrDefault(v => v.Identifier == node.Identifier.Value);
        if (variable == null)
        {
            throw new Exception($"Variable {node.Identifier.Value} not found");
        }
        return new NumbOutput(variable.Value);
        
    }
    Output VisitBinaryOp(BinaryOpNode node)
    {
        Output left = Interpret(node.Left);
        Output right = Interpret(node.Right);
        if (left is NumbOutput leftOutput&&right is NumbOutput rightOutput)
        {
            switch (node.Op?.Type)
            {
                case TokenType.TtPlus:
                    return new NumbOutput(leftOutput.Value + rightOutput.Value);
                case TokenType.TtMinus:
                    return new NumbOutput(leftOutput.Value - rightOutput.Value);
                case TokenType.TtMul:
                    return new NumbOutput(leftOutput.Value * rightOutput.Value); 
                case TokenType.TtDiv:
                    return  rightOutput.Value != 0 ? new NumbOutput(leftOutput.Value/rightOutput.Value) : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {node.Op?.Type}");
            }
        }
        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");
    }
}