using Flow.classes.Output;
using Flow.Nodes;

namespace Flow;

public class Variable: Output
{
    public string? Value { get; set; }
    public string Identifier { get; }
    public bool? boolValue { get; set; }
    public Variable(string identifier, string value)
    {
        Value = value;
        Identifier = identifier;
    }

    public Variable(string identifier, bool? value)
    {
        Identifier = identifier;
        boolValue = value;
    }
    


    public override string ToString()
    {
        return $"{Identifier} = {Value}";
    }
}

public static class VariableManagement
{
    public static readonly List<Variable?> Variables = new List<Variable?>();
    
    public static Output? AddVariable(VariableNode varNode)
    {
        var var = VariableManagement.Variables.FirstOrDefault(v => v != null && v.Identifier == varNode.Identifier.Value);
        Output? output = varNode.Value.VisitNode();
        if (var is null)
        {
            if (output is ValueOutput valueOutput)
            {
                if (varNode.Identifier.Value != null)
                {
                    Variable variable = null;
                    if (valueOutput.BoolValue is not null)
                    {
                        variable = new Variable(varNode.Identifier.Value, valueOutput.BoolValue);    
                    }
                    else
                    {
                        variable = new Variable(varNode.Identifier.Value, valueOutput.Value);
                    }
                    Variables.Add(variable);
                    return new Output();
                }
            }

        }
        else
        {

            if (output is ValueOutput valueOutput)
            {
                var.Value=valueOutput.Value;
                return null;
            }
                
            
        }
        

        throw new OutputError("Invalid value in variable assignment.");
    }
}