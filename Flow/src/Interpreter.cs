using System.Globalization;

namespace Flow;

/*
 * Errors 
 */
public class OutputError(string message) : Exception
{
    public override string ToString()
    {
        return message;
    }
}
/*
 * Nodes
 */
public class Output
{
    /*
     * Doesn't do anything, just uniting things
     */
}

public class NumbOutput(float value) : Output
{
    public readonly float Value = value;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.CurrentCulture);
    }
}



public class Interpreter
{
    

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
            return VisitAccessNode(accNode);
        }
        else if (node is PrintNode printNode)
        {
            var value = Interpret(printNode.Expression);
            Console.WriteLine(value);
            return new Output();
        }

        throw new OutputError($"VisitNode {node.GetType()} not implemented");
    }

    Output CreateVar(VariableNode varNode)
    {
        return VariableManagement.AddVariable(varNode);
    }

    NumbOutput VisitNumber(Node node)
    {
        if (node is NumberNode numberNode)
        {
            return new NumbOutput(float.Parse(numberNode.Token?.Value, CultureInfo.InvariantCulture.NumberFormat));
        }

        throw new OutputError($"VisitNode {node.GetType()} not implemented");
    }

    Output VisitAccessNode(VariableAccessNode node)
    {
        var variable = VariableManagement.Variables.FirstOrDefault(v => v.Identifier == node.Identifier.Value);
        if (variable is null)
        {
            throw new OutputError($"Variable {node.Identifier.Value} not found");
        }

        return new NumbOutput(variable.Value);
    }

    Output VisitBinaryOp(BinaryOpNode node)
    {
        Output left = Interpret(node.Left);
        Output right = Interpret(node.Right);
        if (left is NumbOutput leftOutput && right is NumbOutput rightOutput)
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
                    return rightOutput.Value != 0
                        ? new NumbOutput(leftOutput.Value / rightOutput.Value)
                        : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {node.Op?.Type}");
            }
        }

        throw new OutputError($"VisitNode {node.GetType()} not implemented");
    }
}