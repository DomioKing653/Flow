using Flow.classes;

namespace Flow.Nodes;

class PrintNode(Node expression) : Node
{
    private Node Expression { get; } = expression;

    public override Output VisitNode()
    {
        var value = Expression.VisitNode();
        Console.WriteLine(value);
        return new Output();
    }
}