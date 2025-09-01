using System.Globalization;
using Flow.classes.Output;

namespace Flow.classes.Nodes
{
    /*
     * Value node
     */
    class ValueNode(Token? token) : Node
    {
        private Token? Token { get; set; } = token;
        public override string ToString()
        {
            return $"({Token})";
        }
        public override Output.Output VisitNode()
        {
            return new ValueOutput(Token?.Value);
        }
    }
}