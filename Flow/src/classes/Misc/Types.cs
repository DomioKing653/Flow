namespace Flow;

public enum DataType
{
    Boolean,
    Number,
    String
}

public enum BooleanType
{
    True,
    False
}

public enum TokenType
{
    //Types
    String,
    Float,
    Int,
    Boolean,

    //Math Op.
    Plus,
    Minus,
    Multiply,
    Divide,
    Lparen,
    Rparen,
    Equal,

    //Keywords
    Let,
    While,
    If,

    //Functions
    Println,
    Input,

    //Misc.
    Identifier,
    Semicolon,
    OpeningParenthesis,
    ClosingParenthesis,

    //End
    Eof,
}