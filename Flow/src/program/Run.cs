using Flow.classes;
using Flow.Program;

namespace Flow;

/*
 * Run
 */

static class Run
{
    public static void Main(string[]? args)
    {
        void ProgramLoop(string? code)
        {
            Lexer lexer = new Lexer(code);
            List<Token> tokens = lexer.Tokenize();
            Parser parser = new Parser(tokens);
            Node? ast = parser.Parse();
            Interpreter interpreter = new Interpreter();
            interpreter.Interpret(ast);
        }
        
        var fileName = @"C:\Users\simon\RiderProjects\Flow\Flow\pl\Test.txt";
        if (args is { Length: > 0 } && args[0] != "")
        {
            if (File.Exists(args[0]))
            {
                fileName = args[0];
            }
        }
        var exit = false;
        Console.WriteLine("Welcome to the Flow! Write 'help' for more info");
        while (!exit)
        {
            try
            {
                var code = UserInput.Shell();
                switch (code)
                {
                    case "exit":
                        exit = true;
                        break;
                    case "help":
                        Console.WriteLine("Usage: Flow.exe <code>, write f/F to run the <code>");
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "f":
                    case "F":
                    {

                        Console.WriteLine("Parsing: " + fileName);
                        var input = File.ReadAllText(fileName);
                        Console.WriteLine(input);
                        Console.WriteLine("##############################################");
                        ProgramLoop(input);
                        break;
                    }
                    default:
                    {
                        ProgramLoop(code);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}