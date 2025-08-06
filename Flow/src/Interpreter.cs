namespace Flow;

public class Interpreter
{
    private readonly BinaryOpNode _ast = null!;
    private string? _final;
    public string? Interpret()
    {
        while (true)
        {
            if (_ast.Left is BinaryOpNode)
            {
                _final = _ast.Left.ToString();
                break;
            }
        }

        return _final;
    }
}