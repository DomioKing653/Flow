using Flow.classes.Output;
using Flow.Nodes;

namespace Flow;

public abstract class Variable
{
    public abstract bool Used { get; set; }
    public abstract string Id { get; set; }
}

public class Argument(string id,bool used):Variable
{
    public override bool Used { get; set; } = used;
    public override string Id { get; set; } = id;
}
public class NormalVariable:Variable
{
    
    public override bool Used{get;set;}
    public float? FltValue;
    public string? Value { get; set; }
    public override string Id { get; set; }
    public BooleanType? BoolValue { get; set; }

    public NormalVariable(string id, string value)
    {
        Value = value;
        Id = id;
    }

    public NormalVariable(string id, BooleanType? value)
    {
        Id = id;
        BoolValue = value;
    }

    public NormalVariable(string id, float? value)
    {
        Id = id;
        FltValue = value;
    }


    public override string ToString()
    {
        return $"{Id} = {Value}";
    }
}

public static class VariableManagement
{
    public static readonly List<Variable?> Variables = new List<Variable?>();

    public static Output? AddVariable(VariableNode? varNode,VariableType varType,Argument? arg)
    {
        if (varNode == null)
        {
            if (varType == VariableType.Argument)
            {
                Variables.Add(arg);
            }
        }
        var var =
            VariableManagement.Variables.FirstOrDefault(v => v != null && v.Id == varNode.Identifier.Value);
        Output? output = varNode.Value.VisitNode();
        if (var is null)
        {
            if (output is ValueOutput valueOutput)
            {
                if (varNode.Identifier.Value != null)
                {
                    NormalVariable normalVariable = null;
                    if (valueOutput.BoolValue is not null)
                    {
                        normalVariable = new NormalVariable(varNode.Identifier.Value, valueOutput.BoolValue);
                    }
                    else if (valueOutput.Value is not null)
                    {
                        normalVariable = new NormalVariable(varNode.Identifier.Value, valueOutput.Value);
                    }
                    else
                    {
                        normalVariable = new NormalVariable(varNode.Identifier.Value, valueOutput.FloatValue);
                    }

                    normalVariable.Used = false;
                    Variables.Add(normalVariable);
                    return new Output();
                }
            }
        }
        else
        {
            if (var is  NormalVariable normalVariable)
            {
                if (output is ValueOutput valueOutput)
                {
                    if (valueOutput.BoolValue is not null)
                        normalVariable.BoolValue = valueOutput.BoolValue;
                    else if (valueOutput.Value is not null)
                        normalVariable.Value = valueOutput.Value;
                    else if (valueOutput.FloatValue is not null)
                        normalVariable.FltValue = valueOutput.FloatValue;
                    return null;
                }    
            }
            
        }


        throw new OutputError("Invalid value in variable assignment.");
    }
}