namespace Flow;


/*
 * Nodes
 */
public class Output
{
        
}

public class NumbOutput : Output
{
    public double value;

    public NumbOutput(double value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return $"Solution is {value}";
    }
}
public class Variable : Output
{
    public double Value { get; }
    public string Identifier { get; }

    public Variable(string identifier, double value)
    {
        Value = value;
        Identifier = identifier;
    }

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
        else  if (node is PrintNode prnt)
        {
            
        }
        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");    
       
    }
    Variable CreateVar(VariableNode varNode)
    {
        Output output = Interpret(varNode.Value);

        if (output is NumbOutput numbOutput)
        {
            var variable = new Variable(varNode.Identifier.Value, numbOutput.value);
            _variables.Add(variable);
            return variable;
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

    Output VisitBinaryOp(BinaryOpNode node)
    {
        Output left = Interpret(node.Left);
        Output right = Interpret(node.Right);
        if (left is NumbOutput numbOutputleft&&right is NumbOutput numbOutputright)
        {
            switch (node.Op?.Type)
            {
                case TokenType.TtPlus:
                    return new NumbOutput(numbOutputleft.value + numbOutputright.value);
                case TokenType.TtMinus:
                    return new NumbOutput(numbOutputleft.value - numbOutputright.value); ;
                case TokenType.TtMul:
                    return new NumbOutput(numbOutputleft.value * numbOutputright.value); ;
                case TokenType.TtDiv:
                    return  numbOutputright.value != 0 ? new NumbOutput(numbOutputleft.value/numbOutputright.value) : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {node.Op?.Type}");
            }
        }
        throw new NotImplementedException($"VisitNode {node.GetType()} not implemented");
    }
}