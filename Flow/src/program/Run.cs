using System.Diagnostics;
using Flow.classes;
using Flow.Program;
using Flow.WarningManager;

namespace Flow.program;

internal static class Run
{
    private static Task? _spinnerLexer;
    private static Task? _spinnerParser;

    static async Task ProgramLoop(string? code)
    {
        CancellationTokenSource? ctsLexer = null;
        CancellationTokenSource? ctsParser = null;

        try
        {
            /*
             * Lexer
             */
            ctsLexer = new CancellationTokenSource();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Stopwatch swLexer = Stopwatch.StartNew();
            _spinnerLexer = Task.Run(() => Spinner.Spin("Tokenizing", ctsLexer.Token));
            Lexer lexer = new Lexer(code);
            List<Token> tokens = lexer.Tokenize();
            ctsLexer.Cancel();
            await _spinnerLexer;
            swLexer.Stop();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(swLexer.Elapsed);

            /*
             * Parser
             */
            ctsParser = new CancellationTokenSource();
            Console.ForegroundColor = ConsoleColor.Blue;
            Stopwatch swParser = Stopwatch.StartNew();
            _spinnerParser = Task.Run(() => Spinner.Spin("Parsing", ctsParser.Token));
            Parser parser = new Parser(tokens);
            Node? ast = parser.Parse();
            ctsParser.Cancel();
            await _spinnerParser;
            swParser.Stop();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   " + swParser.Elapsed);
            Console.WriteLine();

            /*
             * Interpreter
             */
            Stopwatch programStopwatch = Stopwatch.StartNew();
            Interpreter interpreter = new Interpreter();
            interpreter.Interpret(ast);
            programStopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" Program finished in {programStopwatch.Elapsed}!");
            Console.ResetColor();
            /*
             * Warnings
             */
            WarningsCreator.CrateWarnings(VariableManagement.Variables);
        }
        finally
        {
            if (ctsLexer != null) await ctsLexer.CancelAsync();
            if (ctsParser != null) await ctsParser.CancelAsync();

            if (_spinnerLexer != null) await _spinnerLexer;
            if (_spinnerParser != null) await _spinnerParser;
        }
    }


    public static async Task Main(string[]? args)
    {
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
                Console.ForegroundColor = ConsoleColor.White;
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
                        var input = await File.ReadAllTextAsync(fileName);
                        Console.WriteLine("Parsing: " + fileName);
                        Console.WriteLine(input);
                        Console.WriteLine("##############################################");
                        await ProgramLoop(input);
                        break;
                    }
                    default:
                    {
                        await ProgramLoop(code);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                if (_spinnerLexer != null)
                {
                    await _spinnerLexer;
                }

                if (_spinnerParser != null)
                {
                    await _spinnerParser;
                }

                Console.WriteLine();
                Console.ResetColor();
                Console.WriteLine(
                    "╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"║{e}");
                Console.ResetColor();
                Console.WriteLine(
                    "╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
            }
        }
    }
}