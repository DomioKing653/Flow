using Flow.Nodes;

namespace Flow.classes.Nodes.KeywordBranches;

public class FunctionNode(List<string> args):ProgramListNode
{
    public override List<Node> Nodes{ get; } 

    public override Output.Output? VisitNode()
    {
        foreach (var arg in args)
        {
            
        }
        foreach (var node in Nodes)
        {
            node.VisitNode();
        }
        return null;
    }
}