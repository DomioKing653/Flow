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
                Environment.Exit(2);
            }
            else
            {
                Lexer lexer = new Lexer(code);
                List<Token> tokens = lexer.Tokenize();
                Parser parser = new Parser(tokens);
                Node ast = parser.Parse();
                Interpreter interpreter = new Interpreter();
                interpreter.Interpret(ast);
                Main(null);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Main(null);
        }
    }
}