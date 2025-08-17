using System.Globalization;

namespace Flow.classes.Nodes
{
    public class InputNode : Node
    {
        public override Output VisitNode()
        {
            Console.Write("Enter Input: ");
            return new NumbOutput(float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture));
        }
    }
}