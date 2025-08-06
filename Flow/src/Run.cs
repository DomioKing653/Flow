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
                Console.WriteLine('\n');
                Console.WriteLine(parser.Parse().ToString());
                Node ast = parser.Parse();


                Main(null);
            }
        }
        catch (SyntaxError e)
        {
            Console.WriteLine(e);
            Main(null);
        }
    }
}