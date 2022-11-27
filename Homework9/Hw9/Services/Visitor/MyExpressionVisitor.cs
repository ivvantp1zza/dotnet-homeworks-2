using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw9.ErrorMessages;

namespace Hw9.Services.Visitor;

[ExcludeFromCodeCoverage]
public class MyExpressionVisitor : ExpressionVisitor
{
    protected override Expression VisitBinary(BinaryExpression root)
    {
        var result = CompileAsync(root.Left, root.Right).Result;
        
        return root.NodeType switch
        {
            ExpressionType.Add => Expression.Add(Expression.Constant(result[0]), Expression.Constant(result[1])),
            ExpressionType.Subtract => Expression.Subtract(Expression.Constant(result[0]), Expression.Constant(result[1])),
            ExpressionType.Multiply => Expression.Multiply(Expression.Constant(result[0]), Expression.Constant(result[1])),
            _ => result[1] < double.Epsilon
                ? throw new Exception(MathErrorMessager.DivisionByZero)
                : Expression.Divide(Expression.Constant(result[0]), Expression.Constant(result[1]))
        };
    }
    public Task<Expression> MyVisit(Expression expr) =>
        Task.Run(() => base.Visit(expr));

    private async Task<double[]> CompileAsync(Expression left, Expression right)
    {
        var t = Task.Run(async() =>
        {
            await Task.Delay(3000);
            return Expression.Lambda<Func<double>>(left).Compile().Invoke();
        });
        var t1 = Task.Run(async() =>
        {
            await Task.Delay(3000);
            return Expression.Lambda<Func<double>>(right).Compile().Invoke();
        });
        return await Task.WhenAll(t, t1);
    }
}