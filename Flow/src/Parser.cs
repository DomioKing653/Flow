namespace Flow;

/*
 * Error
 */
public class SyntaxError(string expected,string details):Exception
{
    public override string ToString()
    {
        return $"Syntax error: expected {expected} found:{details}";
    }
}

/*
 * Nodes
 */
public class Node
{
    /*
     * It's kinda useless just for uniting all Node classes
     * Maybe something else soon
     */
}
class NumberNode(Token? token) : Node
{
    public override string ToString()
    {
        return $"({token})";
    }
}

class BinaryOpNode(Node left, Token? opTok, Node right) : Node
{
    public override string ToString()
    {
        return $"({left} {opTok} {right})";
    }
}

/*
 * Parser
 */
public class Parser
{
    int _tokenIdx;
    readonly List<Token> _tokens;
    Token? _currentToken;

    public Parser(List<Token> tokens)
    {
        this._tokens = tokens;
        _tokenIdx = -1;
        NextToken();
    }

    /*
     * Advancing token idx
     */
    void NextToken()
    {
        _tokenIdx++;
        _currentToken = _tokenIdx < _tokens.Count ? _tokens[_tokenIdx] : null;

    }


    public Node Parse()
    { 
        Node res = Expr(); 
        return res;
    }

    Node Factor()
    {
        string[] numbs = [TokenType.TtFLo, TokenType.TtInt];
        if (_currentToken != null && numbs.Contains(_currentToken.Type))
        {
            return new NumberNode(_currentToken);    
        }
        else
        {
            if (_currentToken == null)
            {
                throw new Exception("End of file");
            }
            throw new SyntaxError("Float or Int", _currentToken.ToString());    
        }
        
    }

    Node Term()
    {
        string[] ops = [TokenType.TtMul, TokenType.TtDiv];
        Node left = Factor();
        NextToken();
        while (_currentToken != null && ops.Contains(_currentToken.Type))
        {
            Token? opToken = _currentToken;
            NextToken();
            Node right = Factor();
            left = new BinaryOpNode(left, opToken, right);
            NextToken();
        }

        return left;
    }
    Node Expr()
    {
        string[] ops = [TokenType.TtPlus, TokenType.TtMinus];
        Node left = Term();
        while (_currentToken != null && ops.Contains(_currentToken.Type))
        {
            Token? opToken = _currentToken;
            NextToken();
            Node right = Term();
            left = new BinaryOpNode(left, opToken, right);
            NextToken();
        }

        return left;
    }
}