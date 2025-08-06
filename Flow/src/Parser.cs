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

class MyClass
{
    
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
class NumberNode : Node
{
    public Token? Token{get;set;}

    public NumberNode(Token? token)
    {
        this.Token = token;
    }
    public override string ToString()
    {
        return $"({Token})";
    }
}

class BinaryOpNode(Node left, Token? opTok, Node right) : Node
{
    public readonly Node Left = left;
    public readonly Node Right = right;
    public readonly Token? Op = opTok;
    public override string ToString()
    {
        return $"({Left} {Op} {Right})";
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
        if (_tokenIdx < _tokens.Count)
        {
            _currentToken = _tokens[_tokenIdx];
        }
        else
        {
            _currentToken = new Token(TokenType.TtEof, null);
        }
    }


    public Node Parse()
    {
        Node res = Expr();

        if (_currentToken.Type != TokenType.TtEof)
        {
            throw new Exception("Expected end of input (EOF) but found extra tokens");
        }

        return res;
    }

    Node Factor()
    {
        string[] numbs = [TokenType.TtFLo, TokenType.TtInt];
        if (_currentToken != null && numbs.Contains(_currentToken.Type))
        {
            var node=new NumberNode(_currentToken);
            NextToken();
            return node;    
        }
        else
        {
            throw new SyntaxError("Float or Int", _currentToken.ToString());    
        }
        
    }

    Node Term()
    {
        string[] ops = { TokenType.TtPlus, TokenType.TtMinus };
        Node left = Factor();

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
        string[] ops = { TokenType.TtPlus, TokenType.TtMinus };
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