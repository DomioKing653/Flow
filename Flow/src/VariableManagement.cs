namespace Flow;

public class Variable(string identifier, float value) : Output
{
    public float Value { get; } = value;
    public string Identifier { get; } = identifier;

    public override string ToString()
    {
        return $"{Identifier} = {Value}";
    }
}

public static class VariableManagement
{
    public static readonly List<Variable?> Variables = new List<Variable?>();

    public static Output AddVariable(VariableNode varNode)
    {
        Interpreter interpreter = new Interpreter();
        Output output = interpreter.Interpret(varNode.Value);

        if (output is NumbOutput numbOutput)
        {
            if (varNode.Identifier.Value != null)
            {
                var variable = new Variable(varNode.Identifier.Value, numbOutput.Value);
                Variables.Add(variable);
                return new Output();
            }
        }

        throw new OutputError("Invalid value in variable assignment.");
    }
}