using Flow.classes.Output;

namespace Flow.classes.Nodes;

public class ConvertToFloat(Node value) : Node
{
    public override Output.Output? VisitNode()
    {
        if (value.VisitNode() is ValueOutput { Value: not null } valOutput)
        {
            float convertedValue = float.Parse(valOutput.Value);
            return new ValueOutput(convertedValue);
        }

        throw new NotImplementedException();
    }
}