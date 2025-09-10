namespace Flow.program;

public class Spinner
{
    public static void Spin(string message, CancellationToken token)
    {
        string[] spinner = new string[] { "|", "/", "-", "\\" };
        int index = 0;

        while (!token.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(spinner[index] + " " + message + "   "); // 3 mezery pro vymazání starého znaku
            index = (index + 1) % spinner.Length;
            Thread.Sleep(100);
        }
    }
}