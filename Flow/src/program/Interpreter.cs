using System.Globalization;
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

/*
 * Nodes
 */
public class Output
{
    /*
     * Doesn't do anything, just uniting things
     */
}

public class StrOutput(string value) : Output
{
    public readonly string Value = value;

    public override string ToString()
    {
        return Value;
    }
}

public class Interpreter
{
    public void Interpret(Node node)
    {
        node.VisitNode();
    }
}