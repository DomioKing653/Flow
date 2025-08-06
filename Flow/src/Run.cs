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
                Node AST=parser.Parse();
                Console.WriteLine(AST.ToString());
                Interpreter interpreter=new Interpreter();
                double result=interpreter.Visit(AST);
                Console.WriteLine(result);
                
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