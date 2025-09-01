namespace Flow.classes.Output;

public class ValueOutput : Output
{
    public readonly string? Value;
    public readonly bool? BoolValue;
    public ValueOutput(string value)
    {
        Value = value;
    }

    public ValueOutput(bool value)
    {
        BoolValue = value;
    }
    public override string ToString()
    {
        return Value;
    }
}