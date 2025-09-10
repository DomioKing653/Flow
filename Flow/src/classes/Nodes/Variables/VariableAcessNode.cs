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
        var variable = VariableManagement.Variables.FirstOrDefault(v => v != null && v.Id == Identifier.Value);
        if (variable is null)
        {
            throw new OutputError($"Variable {Identifier.Value} not found");
        }

        if (variable is NormalVariable varNormal)
        {
            variable.Used = true;
            if (varNormal.BoolValue is not null)
                return new ValueOutput(varNormal.BoolValue);
            else if (varNormal.FltValue is not null)
                return new ValueOutput(varNormal.FltValue);
            else
                return new ValueOutput(varNormal.Value);    
        }
        throw new OutputError($"Variable {Identifier.Value} not found");
    }
}