using Flow.classes;

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



public class Interpreter
{
    public void Interpret(Node? node)
    {
        node?.VisitNode();
    }
}