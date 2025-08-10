using Flow.classes;

namespace Flow.Nodes;

public class VariableNode(Token identifier, Node value) : Node
{
    public Token Identifier { get; set; } = identifier;
    public Node Value { get; set; } = value;

    public override string ToString()
    {
        return $"{Identifier.Value} = {Value}";
    }

    public override Output VisitNode()
    {
        return VariableManagement.AddVariable(this);
    }
}