namespace Flow;

/*
 * Errors
 */
public class Error(string name, string message):Exception
{
    public override string Message { get; } = message;
    private string Name { get; set; } = name;

    public override string ToString()
    {
        return $"{Name}: {Message}";
    }
}
/*
 * Tokens
 */
public static class TokenType
{
    /*
     * Keywords
     */
    public const string TtVarKw = "VAR";
    public const string TtPrintKw="PRINT";
    public const string TtIdentifier = "ID";
    public const string TtSemicolon = "SEMICOLON";
    /*
     * Math op.
     */
    public const string TtPlus = "PLUS";
    public const string TtMinus = "MINUS";
    public const string TtMul = "MULTIPLY";
    public const string TtDiv = "DIVIDE";
    public const string TtRParen = "RPAREN";
    public const string TtLParen = "LPAREN";
    public const string TtInt = "INT";
    public const string TtFLo = "FLOAT";
    public const string TtEof = "EOF";
    public const string TtEqual = "EQUAL";
    
    
}

public class Token(string type, string? value)
{
    public readonly string Type = type;
    public readonly string? Value = value;

    public override string ToString()
    {
        if (Value != null)
        {
            return $"{Type} -> {Value}";
        }
        else
        {
            return $"{Type}";
        }
    }
}

/*
 *Lexer
 */
public class Lexer
{
    readonly string? _text;
    private int _tokenIdx;
    private char _currentToken;
    private readonly List<Token> _tokens;

    public Lexer(string? input)
    {
        this._text = input;
        this._tokenIdx = -1;
        NextToken();
        _tokens = new List<Token>();
    }

    /*
     * Creating numbers
     */
    private void MakeNumber()
    {
        string number = "";
        int dotCount = 0;
        while (int.TryParse(_currentToken.ToString(), out int _) || _currentToken == '.')
        {
            if (_currentToken == '.')
            {
                if (dotCount > 1)
                {
                    break;
                }

                dotCount++;
                number += _currentToken.ToString();
                NextToken();
            }
            else
            {
                number += _currentToken.ToString();
                NextToken();
            }
        }

        if (dotCount == 0)
        {
            _tokens.Add(new Token(TokenType.TtInt, number));
            return;
        }

        if (dotCount > 1)
        {
            throw new Error("Illegal character",_currentToken.ToString());
        }
        else
        {
            _tokens.Add(new Token(TokenType.TtFLo, number));
        }
    }
    /*
     * Making text token like: var, print.
     */
    private void MakeText()
    {
        string text = "";
        while (char.IsLetter(_currentToken))
        {
            text += _currentToken;
            NextToken();
        }

        switch (text)
        {
            case "var":
                _tokens.Add(new Token(TokenType.TtVarKw, null));
                break;
            case "print":
                _tokens.Add(new Token(TokenType.TtPrintKw, null));
                
                break;
            default:
                _tokens.Add(new Token(TokenType.TtIdentifier, text));
                break;
        }

    }

    /*
     * Creating tokens
     */
    public List<Token> Tokenize()
    {
        while (_currentToken != '\0')
        {
            switch (_currentToken)
            {
                case '+':
                    _tokens.Add(new Token(TokenType.TtPlus, null));
                    NextToken();
                    break;
                case '-':
                    _tokens.Add(new Token(TokenType.TtMinus, null));
                    NextToken();
                    break;
                case ')':
                    _tokens.Add(new Token(TokenType.TtRParen, null));
                    NextToken();
                    break;
                case '(':
                    _tokens.Add(new Token(TokenType.TtLParen, null));
                    NextToken();
                    break;
                case '*':
                    _tokens.Add(new Token(TokenType.TtMul, null));
                    NextToken();
                    break;
                case '/':
                    _tokens.Add(new Token(TokenType.TtDiv, null));
                    NextToken();
                    break;
                case '=':
                    _tokens.Add(new Token(TokenType.TtEqual, null));
                    NextToken();
                    break;
                case ';':
                    _tokens.Add(new Token(TokenType.TtSemicolon, null));
                    NextToken();
                    break;
                default:
                    bool isNumber = int.TryParse(_currentToken.ToString(), out int _); 
                    bool isLetter = char.IsLetter(_currentToken);
                    if (isLetter)
                    {
                        MakeText();
                    }
                    else if (isNumber)
                    {
                        MakeNumber();
                    }
                    else if (char.IsWhiteSpace(_currentToken))
                    {
                        NextToken();
                    }
                    else
                    {
                        throw new Error("Illegal character", _currentToken.ToString());
                    }
                    break;
            }
        }
        _tokens.Add(new Token(TokenType.TtEof, null));

        return _tokens;
    }

    /*
     * Advancing position
     */
    void NextToken()
    {
        _tokenIdx++;
        _currentToken = _text != null && _tokenIdx < _text.Length ? _text[_tokenIdx] : '\0';
    }
}