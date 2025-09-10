using Flow.classes;
using Flow.classes.Output;

namespace Flow.Nodes;

class PrintNode(Node expression) : Node
{
    private Node Expression { get; } = expression;

    public override Output VisitNode()
    {
        var value = Expression.VisitNode();
        if (value is ValueOutput val)
        {
            if (val.Value is not null)
            {
                Console.WriteLine(val.Value);        
            }else if(val.FloatValue is not null)
            {
                Console.WriteLine(val.FloatValue);
            }
            else if(val.Value is not null)
            {
                Console.WriteLine(val.BoolValue);
            }
        }
        
        return new Output();
    }
}