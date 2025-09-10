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
    Continue,
    Break,
    Compare,
    Fn,

    //Functions
    Println,
    Print,
    Input,
    ConvertToFloat,
    ConvertToBoolean,

    //LogicalOps
    Equals,
    GreaterThan,

    //Misc.
    Identifier,
    Semicolon,
    OpeningParenthesis,
    ClosingParenthesis,
    Comma,

    //End
    Eof,
}

public enum VariableType
{
    Argument,
    Variable,
}