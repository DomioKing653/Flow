using Flow.classes.Output;
using Flow.Program;

namespace Flow.classes.Nodes.Functions;

public class ConvertToBool(Node value) : Node
{
    public override Output.Output? VisitNode()
    {
        if (value.VisitNode() is ValueOutput output)
        {
            if (output.Value == "true")
            {
                return new ValueOutput(BooleanType.True);
            }
            else
            {
                if (output.Value == "false")
                {
                    return new ValueOutput(BooleanType.False);
                }
            }

            throw new OutputError("Cannot convert value to bool");
        }

        throw new OutputError("Cannot convert value to bool");
    }
}