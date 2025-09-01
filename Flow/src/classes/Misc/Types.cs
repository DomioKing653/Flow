namespace Flow;

public enum DataType
{
    Boolean,
    Number,
    String
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
    //Functions
    Println,
    Input,
    Identifier,
    Semicolon,
    //End
    Eof,
}