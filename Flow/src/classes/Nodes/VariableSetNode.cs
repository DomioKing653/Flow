using Flow.classes;
using Flow.classes.Output;

namespace Flow.Nodes;

class VariableSetNode(string? id, Node value) : Node
{
    public string? Identifier { get; } = id;
    public Node Value { get; } = value;
    public override Output? VisitNode()
    {
        throw new NotImplementedException();
    }
}