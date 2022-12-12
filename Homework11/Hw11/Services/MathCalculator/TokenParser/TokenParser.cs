using System.Diagnostics.CodeAnalysis;
using Hw11.ErrorMessages;
using Hw11.Exceptions;
using static Hw11.ErrorMessages.MathErrorMessager;

namespace Hw11.Services.MathCalculator.TokenParser;

public class TokenParser
{
    public static List<Token> Parse(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new Exception(EmptyString);
        var res = new List<Token>();
        var splitted = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var str in splitted)
        {
            var index = 0;
            while (index < str.Length && (IsOperator(str[index]) || IsBracket(str[index])))
            {
                if (IsBracket(str[index]))
                {
                    res.Add(GetBracketToken(str, index));
                }
                else if (IsOperator(str[index]))
                    res.Add(GetOperatorToken(str, index));
                
                index++;
            }

            if (index < str.Length)
            {
                if (char.IsDigit(str[index]))
                {
                    res.Add(GetNumberToken(str, ref index));
                }
                else if (!IsBracketOrOperator(str[index]))
                {
                    throw new InvalidSymbolException(UnknownCharacterMessage(str[index]));
                }
            }
            
            while (index < str.Length && IsBracket(str[index]))
            {
                res.Add(GetBracketToken(str, index));
                index++;
            }
        }
        return res;
    }
    
    private static Token GetNumberToken(string str, ref int position)
    {
        var startIndex = position;
        while (position < str.Length && char.IsDigit(str[position]))
        {
            position++;
        }
        
        if (position < str.Length)
        {
            if (str[position] == '.')
            {
                position++;
                while (char.IsDigit(str[position]))
                {
                    position++;
                }
            }
        }

        if (position < str.Length && !IsBracket(str[position]))
            throw new InvalidSymbolException(NotNumberMessage(str));
        
        return new Token(TokenType.Number, str.Substring(startIndex, position - startIndex));
    }

    public static Token GetOperatorToken(string str, int position)
    {
        return str[position] switch
        {
            '+' => new Token(TokenType.Plus , "+"),
            '-' => str.Length > 1 ? new Token(TokenType.Negate, "-") : new Token(TokenType.Minus, "-"),
            '/' => new Token(TokenType.Divide, "/"),
            '*' => new Token(TokenType.Multiply, "*"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static Token GetBracketToken(string str, int position)
    {
        return str[position] switch
        {
            '(' => new Token(TokenType.OpenBracket, "("),
            ')' => new Token(TokenType.CloseBracket, ")"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool IsOperator(char c)
    {
        return c switch
        {
            '+' => true,
            '-' => true,
            '*' => true,
            '/' => true,
            _ => false
        };
    }

    private static bool IsBracket(char c)
    {
        return c == '(' || c == ')';
    }
    
    [ExcludeFromCodeCoverage]
    private static bool IsBracketOrOperator(char c)
    {
        return IsOperator(c) || IsBracket(c);
    }
}