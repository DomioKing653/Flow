using Flow.classes.Output;
using Flow.Program;

namespace Flow.classes.Nodes;

public class WhileNode(Node expression):ProgramListNode
{
    public override List<Node> Nodes { get; }=new List<Node>();
    public override Output.Output? VisitNode()
    {
        var expr=expression.VisitNode();
        if (expr is ValueOutput value)
        {
            if (value.BoolValue is not null)
            {
                while(value.BoolValue == true)
                {
                    foreach (var node in Nodes)
                    {
                        node.VisitNode();
                    }
                    expr=expression.VisitNode();
                }    
            }
            
            return null;
        }
        else
        {
            return null;   
        }

        throw new SyntaxError("Expression",expression.ToString());
    }
}