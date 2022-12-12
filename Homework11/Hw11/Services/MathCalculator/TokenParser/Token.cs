namespace Hw11.Services.MathCalculator.TokenParser;

public class Token
{
    public TokenType Type { get; }
    
    public string Value { get; }

    public Token(TokenType type, string value = null)
    {
        Type = type;
        Value = value;
    }
    
    public bool IsNumber() => Type == TokenType.Number;
    public bool IsBinaryOperator() =>
        Type switch
        {
            TokenType.Plus => true,
            TokenType.Minus => true,
            TokenType.Multiply => true,
            TokenType.Divide => true,
            _ => false
        };
    public bool IsNegate() => Type == TokenType.Negate;

    public bool IsOperator() => IsBinaryOperator() || IsNegate();
    
    public int GetRank() =>
        Type switch
        {
            TokenType.OpenBracket or TokenType.CloseBracket => -1,
            TokenType.Plus or TokenType.Minus => 0,
            TokenType.Multiply or TokenType.Divide => 1,
            TokenType.Negate => 2,
            _ => throw new ArgumentOutOfRangeException()
        };
}