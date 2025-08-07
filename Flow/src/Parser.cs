namespace Flow;

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
 * Nodes
 */
class PrintNode(Node expression) : Node
{
    public Node Expression { get; } = expression;
}

public class VariableAccessNode(Token identifier) : Node
{
    public Token Identifier { get; } = identifier;

    public override string ToString()
    {
        return Identifier.Value;
    }
}

public class VariableNode(Token identifier, Node value) : Node
{
    public Token Identifier { get; set; } = identifier;
    public Node Value { get; set; } = value;

    public override string ToString()
    {
        return $"{Identifier.Value} = {Value}";
    }
}

public class Node
{
    /*
     * It's kinda useless just for uniting all Node classes
     * Maybe something else soon
     */
}

class NumberNode(Token? token) : Node
{
    public Token? Token { get; set; } = token;

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
        _currentToken = _tokenIdx < _tokens.Count ? _tokens[_tokenIdx] : new Token(TokenType.TtEof, null);
    }


    public Node Parse()
    {
        Node res = Statement();
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
            var node = new NumberNode(_currentToken);
            NextToken();
            return node;
        }

        else if (_currentToken is { Type: TokenType.TtLParen })
        {
            NextToken();
            Node left = Expr();
            if (_currentToken.Type != TokenType.TtRParen)
            {
                throw new SyntaxError("')'", $"But found{_currentToken}");
            }

            NextToken();
            return left;
        }

        if (_currentToken is { Type: TokenType.TtIdentifier })
        {
            var node = new VariableAccessNode(_currentToken);
            NextToken();
            return node;
        }

        throw new SyntaxError("Float or Int", _currentToken.ToString());
    }

    Node Statement()
    {
        if (_currentToken is { Type: TokenType.TtVarKw })
        {
            NextToken();
            if (_currentToken.Type != TokenType.TtIdentifier)
            {
                throw new SyntaxError("Identifier", $"But found{_currentToken}");
            }

            var id = _currentToken;
            NextToken();
            if (_currentToken.Type != TokenType.TtEqual)
            {
                throw new SyntaxError("Equal", $"But found{_currentToken}");
            }

            NextToken();
            Node expr = Expr();
            SemiCheck(_currentToken);
            NextToken();
            return new VariableNode(id, expr);
        }
        else if (_currentToken is { Type: TokenType.TtPrintKw })
        {
            NextToken();
            if (_currentToken.Type != TokenType.TtLParen)
            {
                throw new SyntaxError("(", $"But found{_currentToken}");
            }

            NextToken();
            var expr = Expr();
            if (_currentToken.Type != TokenType.TtRParen)
            {
                throw new SyntaxError("Parenthesis", $"But found{_currentToken}");
            }

            NextToken();
            SemiCheck(_currentToken);
            return new PrintNode(expr);
        }

        return Expr();
    }

    Node Term()
    {
        string[] ops = [TokenType.TtDiv, TokenType.TtMul];
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
        if (token.Type != TokenType.TtSemicolon)
        {
            throw new SyntaxError("Semicolon", $"But found{token}");
        }

        NextToken();
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
        }

        return left;
    }
}