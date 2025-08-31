namespace Flow;

/*
 * Errors
 */
public class Error(string name, string message) : Exception
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
    public const string TtLetKw = "LET";
    /*
     * FUNCTIONS
     */
    public const string TtPrintFn = "PRINTLN";
    public const string TtInputFn = "INPUT";
    /*
     * Misc
     */
    public const string TtIdentifier = "ID";
    public const string TtSemicolon = "SEMICOLON";
    /*
     * Types
     */
    public const string TtInt = "INT";
    public const string TtFLo = "FLOAT";
    public const string TtStr = "STRING";
    /*
     * Math op.
     */
    public const string TtPlus = "PLUS";
    public const string TtMinus = "MINUS";
    public const string TtMul = "MULTIPLY";
    public const string TtDiv = "DIVIDE";
    public const string TtRParen = "RPAREN";
    public const string TtLParen = "LPAREN";

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
        NextChar();
        _tokens = new List<Token>();
    }

    /*
     * Creating numbers
     */
    private void MakeNumber()
    {
        string number = "";
        int dotCount = 0;
        while (int.TryParse(_currentToken.ToString(), out int _) || _currentToken == '.'|| _currentToken==',')
        {
            if (_currentToken == '.'||_currentToken==',')
            {
                if (dotCount > 1)
                {
                    break;
                }

                dotCount++;
                number += '.';
                NextChar();
            }
            else
            {
                number += _currentToken.ToString();
                NextChar();
            }
        }

        if (dotCount == 0)
        {
            _tokens.Add(new Token(TokenType.TtInt, number));
            return;
        }
        if (dotCount > 1)
        {
            throw new Error("Illegal character", _currentToken.ToString());
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
            NextChar();
        }

        switch (text)
        {
            case "let":
                _tokens.Add(new Token(TokenType.TtLetKw, null));
                break;
            case "println":
                _tokens.Add(new Token(TokenType.TtPrintFn, null));
                break;
            case "input":
                _tokens.Add(new Token(TokenType.TtInputFn, null));
                break;
            default:
                _tokens.Add(new Token(TokenType.TtIdentifier, text));
                break;
        }
    }
    /*
     * Creating string
     */
    void MakeString()
    {
        NextChar();
        string stringText = "";
        while (true)
        {
            if (_currentToken == '"')
            {
                _tokens.Add(new Token(TokenType.TtStr, stringText));
                return;
            }
            stringText += _currentToken;
            NextChar();
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
                    NextChar();
                    break;
                case '-':
                    _tokens.Add(new Token(TokenType.TtMinus, null));
                    NextChar();
                    break;
                case ')':
                    _tokens.Add(new Token(TokenType.TtRParen, null));
                    NextChar();
                    break;
                case '(':
                    _tokens.Add(new Token(TokenType.TtLParen, null));
                    NextChar();
                    break;
                case '*':
                    _tokens.Add(new Token(TokenType.TtMul, null));
                    NextChar();
                    break;
                case '/':
                    _tokens.Add(new Token(TokenType.TtDiv, null));
                    NextChar();
                    break;
                case '=':
                    _tokens.Add(new Token(TokenType.TtEqual, null));
                    NextChar();
                    break;
                case ';':
                    _tokens.Add(new Token(TokenType.TtSemicolon, null));
                    NextChar();
                    break;
                case '"':
                    MakeString();
                    NextChar();
                    break;
                case '#':
                    while (_currentToken!='\n')
                    {
                        NextChar();
                    }
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
                        NextChar();
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
    void NextChar()
    {
        _tokenIdx++;
        _currentToken = _text != null && _tokenIdx < _text.Length ? _text[_tokenIdx] : '\0';
    }
}