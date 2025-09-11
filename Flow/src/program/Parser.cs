using Flow.classes;
using Flow.classes.Nodes;
using Flow.classes.Nodes.Functions;
using Flow.classes.Nodes.Functions.FunctionManagment;
using Flow.classes.Nodes.KeywordBranches;
using Flow.classes.Nodes.LogicOperators;
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

            Statement(_root);
        }
    }

    Node Factor()
    {
        if (_currentToken.Type == TokenType.Compare)
        {
            NextToken();
            var valueL = Factor();
            if (_currentToken.Type == TokenType.GreaterThan)
            {
                NextToken();
                return new CompareNode(valueL, TokenType.GreaterThan, Factor());
            }
        }

        if (_currentToken.Type == TokenType.ConvertToBoolean)
        {
            NextToken();
            if (_currentToken.Type != TokenType.Lparen)
            {
                throw new SyntaxError("(", _currentToken.Type.ToString());
            }

            NextToken();
            var value = Factor();
            if (_currentToken.Type != TokenType.Rparen)
            {
                throw new SyntaxError(")", _currentToken.Type.ToString());
            }

            NextToken();
            return new ConvertToBool(value);
        }

        if (_currentToken.Type == TokenType.ConvertToFloat)
        {
            NextToken();
            if (_currentToken.Type != TokenType.Lparen)
            {
                throw new SyntaxError("(", _currentToken.Type.ToString());
            }

            NextToken();
            var value = Factor();
            if (_currentToken.Type != TokenType.Rparen)
            {
                throw new SyntaxError(")", _currentToken.Type.ToString());
            }

            NextToken();
            return new ConvertToFloat(value);
        }

        TokenType[] numbs = [TokenType.Float, TokenType.Int];
        if (_currentToken != null && numbs.Contains(_currentToken.Type))
        {
            var node = new ValueNode(_currentToken, DataType.Number);
            NextToken();
            return node;
        }

        if (_currentToken is { Type: TokenType.Lparen })
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
            var node = new ValueNode(_currentToken, DataType.String);
            NextToken();
            return node;
        }

        if (_currentToken.Type == TokenType.Boolean)
        {
            var node = new ValueNode(_currentToken, DataType.Boolean);
            if (_currentToken.Value == "true")
                node = new ValueNode(_currentToken, DataType.Boolean);
            if (_currentToken.Value == "false")
                node = new ValueNode(_currentToken, DataType.Boolean);
            NextToken();
            return node;
        }

        if (_currentToken is { Type: TokenType.Identifier })
        {
            var id = _currentToken.Value;
            var node = new VariableAccessNode(_currentToken);
            NextToken();
            if (_currentToken.Type == TokenType.Lparen)
            {
                NextToken();
                List<Node> args = new List<Node>();
                while (_currentToken.Type != TokenType.Rparen)
                {
                    args.Add(Factor());
                    if (_currentToken.Type != TokenType.Pipe)
                    {
                        throw new SyntaxError("','", $"{_currentToken}");
                    }

                    NextToken();
                }

                NextToken();
                return new FunctionAccessNode(args, id!);
            }

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
    void Let(ProgramListNode node)
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
        if (_root != null) node.Nodes.Add(new VariableNode(id, expr));
    }

    /*
     * Print
     */
    void Print(ProgramListNode node)
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
            throw new SyntaxError(")", $"{_currentToken}");
        }

        NextToken();
        SemiCheck(_currentToken);
        node.Nodes.Add(new PrintNode(expr2));
    }

    /*
     * Id
     */
    void Identifier(ProgramListNode node)
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
        node.Nodes.Add(new VariableSetNode(id2, expr3));
    }


    /*
     * While
     */
    void While(ProgramListNode node)
    {
        NextToken();
        if (_currentToken.Type != TokenType.Lparen)
        {
            throw new SyntaxError("(", $"{_currentToken}");
        }

        NextToken();
        var expression = Factor();
        if (_currentToken.Type != TokenType.Rparen)
        {
            throw new SyntaxError("')'", $"{_currentToken}");
        }

        NextToken();
        if (_currentToken.Type != TokenType.OpeningParenthesis)
        {
            throw new SyntaxError("'{'", $"{_currentToken}");
        }

        NextToken();
        WhileNode whileNode = new WhileNode(expression);
        while (_currentToken!.Type != TokenType.Eof || _currentToken.Type != TokenType.ClosingParenthesis)
        {
            Statement(whileNode);
            if (_currentToken.Type == TokenType.Eof)
            {
                throw new SyntaxError("'}'", $"{_currentToken}");
            }

            if (_currentToken.Type == TokenType.ClosingParenthesis)
            {
                node.Nodes.Add(whileNode);
                NextToken();
                return;
            }
        }
    }

    private void Function(ProgramListNode node)
    {
        List<string> args = new List<string>();
        NextToken();
        if (_currentToken.Type != TokenType.Identifier)
        {
            throw new SyntaxError("Identifier", $"{_currentToken}");
        }

        var id = _currentToken.Value;
        NextToken();
        if (_currentToken.Type != TokenType.Lparen)
        {
            throw new SyntaxError("(", $"{_currentToken}");
        }

        NextToken();
        while (_currentToken.Type != TokenType.Rparen)
        {
            if (_currentToken.Type == TokenType.Identifier)
            {
                args.Add(_currentToken.Value);
                NextToken();
                if (_currentToken.Type is not TokenType.Pipe)
                {
                    throw new SyntaxError("Comma", $"{_currentToken}");
                }

                NextToken();
            }
            else if (_currentToken.Type == TokenType.Rparen)
            {
                NextToken();
                break;
            }
            else
            {
                throw new SyntaxError("argument", $"{_currentToken}");
            }
        }

        NextToken();
        if (_currentToken.Type != TokenType.OpeningParenthesis)
        {
            throw new SyntaxError("'{'", $"{_currentToken}");
        }

        NextToken();
        var funcNode = new FunctionNode(args, id);
        while (_currentToken.Type != TokenType.ClosingParenthesis)
        {
            Statement(funcNode);
        }

        NextToken();
        node.Nodes.Add(funcNode);
    }

    private void If(ProgramListNode node)
    {
        NextToken();
        if (_currentToken.Type != TokenType.Lparen)
        {
            throw new SyntaxError("(", $"{_currentToken}");
        }

        NextToken();
        var expression = Factor();
        if (_currentToken.Type != TokenType.Rparen)
        {
            throw new SyntaxError("')'", $"{_currentToken}");
        }

        NextToken();
        if (_currentToken.Type != TokenType.OpeningParenthesis)
        {
            throw new SyntaxError("'{'", $"{_currentToken}");
        }

        NextToken();
        IfNode whileNode = new IfNode(expression);
        while (_currentToken!.Type != TokenType.Eof || _currentToken.Type != TokenType.ClosingParenthesis)
        {
            Statement(whileNode);
            if (_currentToken.Type == TokenType.Eof)
            {
                throw new SyntaxError("'}'", $"{_currentToken}");
            }

            if (_currentToken.Type == TokenType.ClosingParenthesis)
            {
                node.Nodes.Add(whileNode);
                NextToken();
                return;
            }
        }
    }

    /*
     * Statement
     */
    void Statement(ProgramListNode listNode)
    {
        switch (_currentToken!.Type)
        {
            case TokenType.Let:
                Let(listNode);
                break;
            case TokenType.Println:
                Print(listNode);
                break;
            case TokenType.Identifier:
                Identifier(listNode);
                break;
            case TokenType.While:
                While(listNode);
                break;
            case TokenType.If:
                If(listNode);
                break;
            case TokenType.Fn:
                Function(listNode);
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