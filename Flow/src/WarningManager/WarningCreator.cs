namespace Flow.WarningManager;

public static class WarningsCreator
{
    public static void CrateWarnings(List<Variable> vars)
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
        foreach (var variable in vars)
        {
            if (variable.Used == true) continue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"║Warning: variable {variable.Identifier} is in current context defined but never used! You can safely delete it or rename it _{variable.Identifier}.║");
            Console.ResetColor();
        }
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
    }
}