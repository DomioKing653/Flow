using System.Globalization;

namespace Flow.classes.Nodes;

class NumberNode(Token? token) : Node
{
    private Token? Token { get; set; } = token;

    public override string ToString()
    {
        return $"({Token})";
    }

    public override Output VisitNode()
    {
        return new NumbOutput(float.Parse(Token?.Value,CultureInfo.InvariantCulture));
    }
}