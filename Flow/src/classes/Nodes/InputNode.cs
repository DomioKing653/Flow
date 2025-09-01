using System.Globalization;
using Flow.classes.Output;

namespace Flow.classes.Nodes
{
    public class InputNode : Node
    {
        public override Output.Output VisitNode()
        {
            Console.Write("Enter Input: ");
            return new ValueOutput(Console.ReadLine());
        }
    }
}
