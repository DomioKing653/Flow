using Flow.classes.Output;
using Flow.Program;

namespace Flow.classes.Nodes;

public class IfNode(Node expression):ProgramListNode
{
    public override List<Node> Nodes { get; }=new List<Node>();
    public override Output.Output VisitNode()
    {
        var expr=expression.VisitNode();
        if (expr is ValueOutput { BoolValue: not null } value)
        {
            if(value.BoolValue == true)
            {
                foreach (var node in Nodes)
                {
                    node.VisitNode();
                }
                expr=expression.VisitNode();
            }
        }
        throw new SyntaxError("Expression",expression.ToString());
    }
}