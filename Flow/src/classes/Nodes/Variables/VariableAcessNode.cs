using Flow.classes;
using Flow.classes.Output;

namespace Flow.Nodes;

public class VariableAccessNode(Token identifier) : Node
{
    private Token Identifier { get; } = identifier;

    public override string? ToString()
    {
        return Identifier.Value;
    }

    public override Output? VisitNode()
    {
        var variable = VariableManagement.Variables.FirstOrDefault(v => v != null && v.Identifier == Identifier.Value);
        if (variable is null)
        {
            throw new OutputError($"Variable {Identifier.Value} not found");
        }

        variable.Used = true;
        if (variable.BoolValue is not null)
            return new ValueOutput(variable.BoolValue);
        else if (variable.FltValue is not null)
            return new ValueOutput(variable.FltValue);
        else
            return new ValueOutput(variable.Value);
    }
}