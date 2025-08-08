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
public abstract class Node
{
    /*
     * It's kinda useless just for uniting all Node classes
     */
    public abstract Output VisitNode();
    
}
public class ProgramNode : Node
{
    private List<Node> programNodes { get; }=new List<Node>();
    public override Output VisitNode()
    {
        throw new NotImplementedException();
    }
}
class VariableSetNode(string id, Node Value) : Node
{
    public string Identifier { get; } = id;
    public Node Value { get; } = Value;
    public override Output VisitNode()
    {
        throw new NotImplementedException();
    }
}

class PrintNode(Node expression) : Node
{
    public Node Expression { get; } = expression;

    public override Output VisitNode()
    {
        var value = Expression.VisitNode();
        Console.WriteLine(value);
        return new Output();
    }
}

public class VariableAccessNode(Token identifier) : Node
{
    public Token Identifier { get; } = identifier;

    public override string ToString()
    {
        return Identifier.Value;
    }

    public override Output VisitNode()
    {
        var variable = VariableManagement.Variables.FirstOrDefault(v => v.Identifier == Identifier.Value);
        if (variable is null)
        {
            throw new OutputError($"Variable {Identifier.Value} not found");
        }

        return new NumbOutput(variable.Value);
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

    public override Output VisitNode()
    {
        return VariableManagement.AddVariable(this);

    }
}
class NumberNode(Token? token) : Node
{
    public Token? Token { get; set; } = token;

    public override string ToString()
    {
        return $"({Token})";
    }

    public override Output VisitNode()
    {
        return new NumbOutput(float.Parse(Token?.Value));
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

    public override Output VisitNode()
    {
        Output left = Left.VisitNode();
        Output right = Left.VisitNode();
        if (left is NumbOutput leftOutput && right is NumbOutput rightOutput)
        {
            switch (Op?.Type)
            {
                case TokenType.TtPlus:
                    return new NumbOutput(leftOutput.Value + rightOutput.Value);
                case TokenType.TtMinus:
                    return new NumbOutput(leftOutput.Value - rightOutput.Value);
                case TokenType.TtMul:
                    return new NumbOutput(leftOutput.Value * rightOutput.Value);
                case TokenType.TtDiv:
                    return rightOutput.Value != 0
                        ? new NumbOutput(leftOutput.Value / rightOutput.Value)
                        : throw new Exception("Division by zero");
                default:
                    throw new Exception($"Unknown operator: {Op?.Type}");
            }
        }

        throw new OutputError($"VisitNode {GetType()} not implemented");
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

        throw new SyntaxError("Float or Int", _currentToken.ToString());
    }

    void Rrogram()
    {
        
    }
    Node Statement()
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
                NextToken();
                return new VariableNode(id, expr);
                break;
            case TokenType.TtPrintKw:
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
                return new PrintNode(expr2);
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
                return new VariableSetNode(id2, expr3);
                break;
                
        }

        

        throw new SyntaxError("Statement", $"{_currentToken}");
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