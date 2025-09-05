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
                    return new ValueOutput(BooleanType.True);        
                }else
                {
                    return new ValueOutput(BooleanType.False);
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