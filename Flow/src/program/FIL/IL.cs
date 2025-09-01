using System.Data;

namespace Flow.program.FIL;

public enum Commands
{
    Load,
    Push,
    Write,
    Output,
    Input,
}
public class Il
{
    private readonly List<Commands> _commands = new List<Commands>();
    public void Run()
    {
        foreach (var command in _commands)
        {
            throw new NotImplementedException();
        }
    }
}