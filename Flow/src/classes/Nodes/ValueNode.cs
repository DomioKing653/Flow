using Flow.classes.Output;

namespace Flow.classes.Nodes
{
    /*
     * Value node
     */
    class ValueNode(Token? token,DataType type) : Node
    {
        private Token? Token { get; set; } = token;
        private DataType Type { get; } = type;
        public override string ToString()
        {
            return $"({Token})";
        }
        public override Output.Output VisitNode()
        {
            if (Type == DataType.Boolean)
            {
                if (Token.Value == "true")
                {
                    return new ValueOutput(true);        
                }else
                {
                    return new ValueOutput(false);
                }
            }
            else
            {
                return new ValueOutput(Token?.Value);
            }
            throw new System.NotImplementedException();
        }
    }
}