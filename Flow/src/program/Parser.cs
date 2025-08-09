using Flow.classes;
using Flow.classes.Nodes;
using Flow.Nodes;

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
 * Parser
 */
public class Parser
{
    ProgramNode root=new ProgramNode();
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
        Node res = Program();
        if (_currentToken.Type != TokenType.TtEof)
        {
            throw new Exception("Expected end of input (EOF) but found extra tokens");
        }
        return res;
    }

    public ProgramNode Program()
    {
        while (true)
        {
            if (_currentToken.Type==TokenType.TtEof)
            {
                return root;
            }
            Statement();    
        }
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
                throw new SyntaxError("')'", $"{_currentToken}");
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
        else if (_currentToken is {Type: TokenType.TtInputFn})
        {
            if (_currentToken.Type != TokenType.TtLParen)
            {
                throw new SyntaxError("'('", $"{_currentToken}");
            }   
            NextToken();
            if (_currentToken.Type != TokenType.TtRParen)
            {
                throw new SyntaxError("')'", $"{_currentToken}");
            }
            SemiCheck(_currentToken);
        }

        throw new SyntaxError("Float or Int", _currentToken.ToString());
    }


    void Statement()
    {
        switch (_currentToken.Type)
        {
            case TokenType.TtVarKw:
                NextToken();
                if (_currentToken.Type != TokenType.TtIdentifier)
                {
                    throw new SyntaxError("Identifier", $"{_currentToken}");
                }

                var id = _currentToken;
                NextToken();
                if (_currentToken.Type != TokenType.TtEqual)
                {
                    throw new SyntaxError("Equal", $"{_currentToken}");
                }
                NextToken();
                Node expr = Expr();
                SemiCheck(_currentToken);
                root.programNodes.Add(new VariableNode(id, expr)); 
                break;
            case TokenType.TtPrintFn:
                NextToken();
                if (_currentToken.Type != TokenType.TtLParen)
                {
                    throw new SyntaxError("(", $"{_currentToken}");
                }
                NextToken();
                var expr2 = Expr();
                if (_currentToken.Type != TokenType.TtRParen)
                {
                    throw new SyntaxError("Parenthesis", $"{_currentToken}");
                }
                NextToken();
                SemiCheck(_currentToken);
                root.programNodes.Add(new PrintNode(expr2));
                break;
            case TokenType.TtIdentifier:
                string id2 = _currentToken.Value;
                NextToken();
                if (_currentToken.Type != TokenType.TtEqual)
                {
                    throw new SyntaxError("Equal", $"{_currentToken}");
                }
                NextToken();
                Node expr3 = Expr();
                NextToken();
                SemiCheck(_currentToken);
                root.programNodes.Add(new VariableSetNode(id2, expr3));
                break;
            default:
                throw new SyntaxError("Statement", $"{_currentToken}");
                
        }
        
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
            throw new SyntaxError("Semicolon", $"{token}");
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