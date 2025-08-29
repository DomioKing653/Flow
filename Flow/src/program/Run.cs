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
        void DoJob(string? code)
        {
            Lexer lexer = new Lexer(code);
            List<Token> tokens = lexer.Tokenize();
            Parser parser = new Parser(tokens);
            Node ast = parser.Parse();
            Interpreter interpreter = new Interpreter();
            interpreter.Interpret(ast);
        }

        var exit = false;
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
                    case "f":
                    case "F":
                    {
                        var fileName = @"C:\Users\simon\RiderProjects\Flow\Flow\pl\Test.txt";
                        if (args is { Length: > 0 } && args[0] != "")
                        {
                            if (File.Exists(args[0]))
                            {
                                fileName = args[0].ToString();
                            }
                        }

                        Console.WriteLine("Parsing: " + fileName);
                        var input = File.ReadAllText(fileName);
                        Console.WriteLine(input);
                        Console.WriteLine("##############################################");
                        DoJob(input);
                        break;
                    }
                    default:
                    {
                        DoJob(code);
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