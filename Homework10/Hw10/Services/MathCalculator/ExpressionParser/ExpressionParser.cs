using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using Hw10.ErrorMessages;
using Hw10.Services.MathCalculator.TokenParser;

namespace Hw10.Services.MathCalculator.ExpressionParser;

[ExcludeFromCodeCoverage]
public class ExpressionParser
{
    public static Expression Parse(string? input)
    {
        var list = TokenParser.TokenParser.Parse(input);
        if (list[0].IsBinaryOperator())
            throw new Exception(MathErrorMessager.StartingWithOperation);
        if (list[list.Count - 1].IsBinaryOperator())
            throw new Exception(MathErrorMessager.EndingWithOperation);

        var bracketCounter = 0;
        for (var i = 0; i < list.Count; i++)
        {

            if (i > 0 && list[i - 1].IsBinaryOperator() && list[i].IsBinaryOperator())
            {
                throw new Exception(MathErrorMessager.TwoOperationInRowMessage(list[i - 1].Value, list[i].Value));
            }

            if (i > 0 && list[i].Type == TokenType.CloseBracket && !(list[i - 1].IsNumber() ||
                                                                     list[i - 1].Type == TokenType.CloseBracket ||
                                                                     list[i - 1].Type == TokenType.OpenBracket))
            {
                throw new Exception(MathErrorMessager.OperationBeforeParenthesisMessage(list[i - 1].Value));
            }

            if (list[i].Type == TokenType.CloseBracket)
            {
                bracketCounter--;
                if (bracketCounter < 0)
                    throw new Exception(MathErrorMessager.IncorrectBracketsNumber);
            }

            if (i < list.Count - 1 && list[i].Type == TokenType.OpenBracket && list[i + 1].IsBinaryOperator())
            {
                throw new Exception(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(list[i + 1].Value));
            }

            if (list[i].Type == TokenType.OpenBracket)
            {
                bracketCounter++;
            }
        }

        if (bracketCounter != 0)
        {
            throw new Exception(MathErrorMessager.IncorrectBracketsNumber);
        }
        
        var exprStack = new Stack<Expression>();
        var opStack = new Stack<Token>();
        foreach (var token in list)
        {
            if (token.Type == TokenType.Number)
            {
                exprStack.Push(Expression.Constant(double.Parse(token.Value, CultureInfo.InvariantCulture)));
                continue;
            }

            if (token.Type == TokenType.OpenBracket)
            {
                opStack.Push(token);
                continue;
            }

            if (token.IsOperator())
            {
                if (opStack.Count != 0 &&
                    token.GetRank() <= opStack.Peek().GetRank())
                {
                    ProcessExpression(token, exprStack, opStack);
                }
                opStack.Push(token);
            }
            else if (token.Type == TokenType.CloseBracket)
            {
                while (opStack.Peek().Type != TokenType.OpenBracket)
                {
                    PushExpression(opStack.Pop(), exprStack);
                }
                opStack.Pop();
            }
        }
        while (opStack.Count != 0)
        {
            PushExpression(opStack.Pop(), exprStack);
        }
        return exprStack.Pop();
    }

    private static void ProcessExpression(Token token, Stack<Expression> expressionStack,
        Stack<Token> operatorStack)
    {
        while (operatorStack.Count != 0 && token.GetRank() < operatorStack.Peek().GetRank())
        {
            PushExpression(operatorStack.Pop(), expressionStack);
        }
    }

    private static void PushExpression(Token token, Stack<Expression> exprStack)
    {
        switch (token.Type)
        {
            case TokenType.Minus:
            {
                var secondValue = exprStack.Pop();
                var firstValue = exprStack.Pop();
                exprStack.Push(Expression.Subtract(firstValue, secondValue));
                break;
            }
            case TokenType.Plus:
            {
                exprStack.Push(Expression.Add(exprStack.Pop(), exprStack.Pop()));
                break;
            }
            case TokenType.Multiply:
            {
                exprStack.Push(Expression.Multiply(exprStack.Pop(), exprStack.Pop()));
                break;
            }
            case TokenType.Divide:
            {
                var secondValue = exprStack.Pop();
                var firstValue = exprStack.Pop();
                exprStack.Push(Expression.Divide(firstValue, secondValue));
                break;
            }
            case TokenType.Negate:
            {
                exprStack.Push(Expression.Negate(exprStack.Pop()));
                break;
            }
        }
    }
}