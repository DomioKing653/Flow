using System.Globalization;

namespace Flow.classes.Nodes
{
    /*
     * Number node
     */
    class NumberNode(Token? token) : Node
    {
        private Token? Token { get; set; } = token;

        public override string ToString()
        {
            return $"({Token})";
        }

        public override Output VisitNode()
        {
            string normalized = Token?.Value.Replace(',', '.');
            return new NumbOutput(float.Parse(normalized, CultureInfo.InvariantCulture));
        }
    }
}