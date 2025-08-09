using Flow.classes;

namespace Flow;

/*
 * Run
 */
static class Run
{
    public static void Main(string[]? args)
    {
        try
        {
            string? code = UserInput.Shell();
            if (code == "exit")
            {
                Environment.Exit(100);
            }
            else if(code=="f"||code=="F")
            {
                string? input = File.ReadAllText("C:\\Users\\simon\\RiderProjects\\Flow\\Flow\\pl\\Test.txt");

                Lexer lexer = new Lexer(input);
                List<Token> tokens = lexer.Tokenize();
                Parser parser = new Parser(tokens);
                Node ast = parser.Parse();
                Interpreter interpreter = new Interpreter();
                interpreter.Interpret(ast);
                Main(null);
            }
            else
            {
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Main(null);
        }
    }
}