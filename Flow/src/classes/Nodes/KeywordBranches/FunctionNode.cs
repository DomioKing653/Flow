using Flow.Nodes;

namespace Flow.classes.Nodes.KeywordBranches;

public class FunctionNode(List<string> args):ProgramListNode
{
    public override List<Node> Nodes{ get; } =new List<Node>(); 

    public override Output.Output? VisitNode()
    {
        foreach (var arg in args)
        {
            VariableManagement.AddVariable(null, VariableType.Argument,new Argument(arg,false));
        }
        foreach (var node in Nodes)
        {
            node.VisitNode();
        }
        return null;
    }
}