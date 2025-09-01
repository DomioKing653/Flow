using Flow.classes;
using Flow.classes.Output;

namespace Flow.Nodes;

public class VariableNode(Token identifier, Node value) : Node
{
    public Token Identifier { get;} = identifier;
    public Node Value { get;} = value;
    
    public override string ToString()
    {
        return $"{Identifier.Value} = {Value}";
    }
    public override Output VisitNode()
    {
        return VariableManagement.AddVariable(this);
    }
}