using System.Globalization;

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
        
        public override Output VisitNode()
        {
            string normalized = Token?.Value.Replace(',', '.');
            return new StrOutput(normalized);
        }
    }
}