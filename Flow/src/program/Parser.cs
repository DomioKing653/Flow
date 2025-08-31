using Flow.classes;
using Flow.classes.Nodes;
using Flow.Nodes;

namespace Flow.Program;

/*
 * Error
 */
public class SyntaxError(string expected, string details) : Exception
{
    public override string ToString()
    {
        return $"Syntax error: expected {expected} found:{details}";
    }
}

/*
 * Parser
 */
public class Parser
{
    private readonly ProgramNode? _root = new ProgramNode();
    int _tokenIdx;
    readonly List<Token> _tokens;
    Token? _currentToken;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _tokenIdx = -1;
        NextToken();
    }

    /*
     * Advancing token idx
     */
    void NextToken()
    {
        _tokenIdx++;
        _currentToken = _tokenIdx < _tokens.Count ? _tokens[_tokenIdx] : new Token(TokenType.Eof, null);
    }

    public Node? Parse()
    {
        Node? res = Program();
        if (_currentToken.Type != TokenType.Eof)
        {
            throw new Exception("Expected end of input (EOF) but found extra tokens");
        }

        return res;
    }

    private ProgramNode? Program()
    {
        while (true)
        {
            if (_currentToken.Type == TokenType.Eof)
            {
                return _root;
            }

            Statement();
        }
    }

    Node Factor()
    {
        TokenType[] numbs = [TokenType.Float, TokenType.Int];
        if (_currentToken != null && numbs.Contains(_currentToken.Type))
        {
            var node = new ValueNode(_currentToken);
            NextToken();
            return node;
        }
        else if (_currentToken is { Type: TokenType.Lparen })
        {
            NextToken();
            Node left = Expr();
            if (_currentToken.Type != TokenType.Rparen)
            {
                throw new SyntaxError("')'", $"{_currentToken}");
            }

            NextToken();
            return left;
        }

        if (_currentToken.Type == TokenType.String)
        {
            var node = new ValueNode(_currentToken);
            NextToken();
            return node;
        }

        if (_currentToken is { Type: TokenType.Identifier })
        {
            var node = new VariableAccessNode(_currentToken);
            NextToken();
            return node;
        }

        if (_currentToken is { Type: TokenType.Input })
        {
            NextToken();
            if (_currentToken.Type != TokenType.Lparen)
            {
                throw new SyntaxError("'('", $"{_currentToken}");
            }

            NextToken();
            if (_currentToken.Type != TokenType.Rparen)
            {
                throw new SyntaxError("')'", $"{_currentToken}");
            }

            NextToken();
            return new InputNode();
        }

        throw new SyntaxError("Value", _currentToken.ToString());
    }

    /*
     * Let
     */
    void Let()
    {
        NextToken();
        if (_currentToken.Type != TokenType.Identifier)
        {
            throw new SyntaxError("Identifier", $"{_currentToken}");
        }

        var id = _currentToken;
        NextToken();
        if (_currentToken.Type != TokenType.Equal)
        {
            throw new SyntaxError("Equal", $"{_currentToken}");
        }

        NextToken();
        Node expr = Expr();
        SemiCheck(_currentToken);
        if (_root != null) _root.ProgramNodes.Add(new VariableNode(id, expr));
    }
    /*
     * Print
     */
    void Print()
    {
        NextToken();
        if (_currentToken.Type != TokenType.Lparen)
        {
            throw new SyntaxError("(", $"{_currentToken}");
        }

        NextToken();
        var expr2 = Expr();
        if (_currentToken.Type != TokenType.Rparen)
        {
            throw new SyntaxError("Parenthesis", $"{_currentToken}");
        }

        NextToken();
        SemiCheck(_currentToken);
        _root.ProgramNodes.Add(new PrintNode(expr2));
    }

    /*
     * Id
     */
    void Identifier()
    {
        string? id2 = _currentToken.Value;
        NextToken();
        if (_currentToken.Type != TokenType.Equal)
        {
            throw new SyntaxError("Equal", $"{_currentToken}");
        }

        NextToken();
        Node expr3 = Expr();
        NextToken();
        SemiCheck(_currentToken);
        _root.ProgramNodes.Add(new VariableSetNode(id2, expr3));
    }

    /*
     * Statement
     */
    void Statement()
    {
        switch (_currentToken.Type)
        {
            case TokenType.Let:
                Let();
                break;
            case TokenType.Println:
                Print();
                break;
            case TokenType.Identifier:
                Identifier();
                break;
            default:
                throw new SyntaxError("Statement", $"{_currentToken}");
        }
    }

    Node Term()
    {
        TokenType[] ops = [TokenType.Divide, TokenType.Multiply];
        Node left = Factor();

        while (_currentToken != null && ops.Contains(_currentToken.Type))
        {
            Token? opToken = _currentToken;
            NextToken();
            Node right = Factor();
            left = new BinaryOpNode(left, opToken, right);
        }

        return left;
    }

    void SemiCheck(Token token)
    {
        if (token.Type != TokenType.Semicolon)
        {
            throw new SyntaxError("Semicolon", $"{token}");
        }

        NextToken();
    }

    Node Expr()
    {
        TokenType[] ops = [TokenType.Plus, TokenType.Minus];
        Node left = Term();
        while (_currentToken != null && ops.Contains(_currentToken.Type))
        {
            Token? opToken = _currentToken;
            NextToken();
            Node right = Term();
            left = new BinaryOpNode(left, opToken, right);
        }

        return left;
    }
}