using Flow.classes.Output;
using Flow.Nodes;

namespace Flow;

public class Variable(string identifier, string value) : Output
{
    public string Value { get; } = value;
    public string Identifier { get; } = identifier;
    

    public override string ToString()
    {
        return $"{Identifier} = {Value}";
    }
}

public static class VariableManagement
{
    public static readonly List<Variable?> Variables = new List<Variable?>();
    public static Output AddVariable(VariableNode varNode)
    { 
        Output output = varNode.Value.VisitNode();

        if (output is ValueOutput numbOutput)
        {
            if (varNode.Identifier.Value != null)
            {
                var variable = new Variable(varNode.Identifier.Value, numbOutput.Value);
                Variables.Add(variable);
                return new Output();
            }
        }

        throw new OutputError("Invalid value in variable assignment.");
    }
}