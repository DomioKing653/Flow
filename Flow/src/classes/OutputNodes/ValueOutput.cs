namespace Flow.classes.Output;

public class ValueOutput : Output
{
    public string? Value{get;set;}
    public bool? BoolValue { get; set; }
    public float? FloatValue { get; set; }
    public ValueOutput(string? value)
    {
        Value = value;
    }

    public ValueOutput(bool? value)
    {
        BoolValue = value;
    }

    public ValueOutput(float? value)
    {
        FloatValue = value;
    }



    public override string? ToString()
    {
        return Value;
    }
}