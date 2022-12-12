using System.Globalization;
using System.Linq.Expressions;
using Hw11.ErrorMessages;
using Hw11.Exceptions;
using Hw11.Services.MathCalculator.TokenParser;
using static Hw11.ErrorMessages.MathErrorMessager;

namespace Hw11.Services.MathCalculator.ExpressionParser;

public class ExpressionParser
{
    public static Expression Parse(string? input)
    {
        var list = TokenParser.TokenParser.Parse(input);
        if (list[0].IsBinaryOperator())
            throw new InvalidSyntaxException(StartingWithOperation);
        if (list[list.Count - 1].IsBinaryOperator())
            throw new InvalidSyntaxException(EndingWithOperation);

        var bracketCounter = 0;
        for (var i = 0; i < list.Count; i++)
        {

            if (i > 0 && list[i - 1].IsBinaryOperator() && list[i].IsBinaryOperator())
            {
                throw new InvalidSyntaxException(TwoOperationInRowMessage(list[i - 1].Value, list[i].Value));
            }

            if (i > 0 && list[i].Type == TokenType.CloseBracket && !(list[i - 1].IsNumber() ||
                                                                     list[i - 1].Type == TokenType.CloseBracket ||
                                                                     list[i - 1].Type == TokenType.OpenBracket))
            {
                throw new InvalidSyntaxException(OperationBeforeParenthesisMessage(list[i - 1].Value));
            }

            if (list[i].Type == TokenType.CloseBracket)
            {
                bracketCounter--;
                if (bracketCounter < 0)
                    throw new InvalidSyntaxException(IncorrectBracketsNumber);
            }

            if (i < list.Count - 1 && list[i].Type == TokenType.OpenBracket && list[i + 1].IsBinaryOperator())
            {
                throw new InvalidSyntaxException(InvalidOperatorAfterParenthesisMessage(list[i + 1].Value));
            }

            if (list[i].Type == TokenType.OpenBracket)
            {
                bracketCounter++;
            }
        }

        if (bracketCounter != 0)
        {
            throw new InvalidSyntaxException(IncorrectBracketsNumber);
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