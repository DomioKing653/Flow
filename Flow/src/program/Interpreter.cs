using Flow.classes;
using Flow.classes.Output;

namespace Flow;

/*
 * Errors
 */
public class OutputError(string message) : Exception
{
    public override string ToString()
    {
        return message;
    }
}

/*
 * Nodes
 */



public class BooleanOutput(bool value) : Output
{
    public bool Value { get;set; }=value;
}
public class Interpreter
{
    public void Interpret(Node? node)
    {
        node.VisitNode();
    }
}