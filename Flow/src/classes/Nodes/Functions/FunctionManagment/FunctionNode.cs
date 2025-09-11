using Flow.classes.Nodes.Functions.FunctionManagement;
using Flow.Nodes;

namespace Flow.classes.Nodes.KeywordBranches;

public class FunctionNode(List<string> args,string id):ProgramListNode
{
    public string Id=id;
    public override List<Node> Nodes{ get; } =new List<Node>();

    public List<string> Arguments { get; set; } = args;

    public override Output.Output? VisitNode()
    {
        FuncManager.AddFunc(this);
        return null;
    }
}