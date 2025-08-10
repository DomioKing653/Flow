namespace Flow;

static class UserInput
{
    static string? _consoleInput;

    public static string? Shell()
    {   
        Console.Write("Flow>");
        _consoleInput = Console.ReadLine();
        if (_consoleInput is { Length: 0 })
        {
            Console.WriteLine("Please enter a valid input");
            Shell();
        }

        return _consoleInput;
    }
}