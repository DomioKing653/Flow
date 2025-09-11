using Flow.classes.Nodes.KeywordBranches;

namespace Flow.classes.Nodes.Functions.FunctionManagement;

public class Function(List<string> args,string id,List<Node> statements)
{
    public List<string> Args = args;
    public string Id { get; set; } = id;
    public List<Node> Nodes { get; } = statements;
}
public class FuncManager
{
    public static List<Function?> Functions=new List<Function?>();

    public static void AddFunc(FunctionNode function)
    {
        Functions.Add(new Function(function.Arguments,function.Id,function.Nodes));
        return;
    }
}