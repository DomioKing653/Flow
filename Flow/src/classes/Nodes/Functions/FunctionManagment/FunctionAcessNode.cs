using Flow.classes.Nodes.Functions.FunctionManagement;
using Flow.classes.Output;
using Flow.Nodes;

namespace Flow.classes.Nodes.Functions.FunctionManagment;

public class FunctionAccessNode(List<Node> args, string id) : Node
{
    public override Output.Output? VisitNode()
    {
        Function? variable = FuncManager.Functions.FirstOrDefault(v => v?.Id == id);
        foreach (var variableArg in variable.Args)
        {
            foreach (var arg in args)
            {
                VariableManagement.AddVariable(new VariableNode(new Token(TokenType.Identifier, variableArg), arg));
            }
        }


        if (variable is { } f)
        {
            foreach (var node in f.Nodes)
            {
                node.VisitNode();
            }
        }

        if (variable is not null)
        {
            return new ValueOutput();
        }

        throw new OutputError($"Function [{id}] not found");
    }
}