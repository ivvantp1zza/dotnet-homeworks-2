using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw11.ErrorMessages;

namespace Hw11.Services.Visitor;

[ExcludeFromCodeCoverage]
public static class MyExpressionVisitor 
{
    [ExcludeFromCodeCoverage]
    public static async Task<double> VisitExpression(Expression expr)
    {
        return await Visit((dynamic)expr);
    }

    private static async Task<double> Visit(BinaryExpression binExpr)
    {
        await Task.Delay(1000);
        var t = VisitExpression((dynamic)binExpr.Left);
        var t1 = VisitExpression((dynamic)binExpr.Right);
        var result = await Task.WhenAll(t, t1);
        return binExpr.NodeType switch
        {
            ExpressionType.Add => result[0] + result[1],
            ExpressionType.Subtract => result[0] - result[1],
            ExpressionType.Multiply => result[0] * result[1],
            _ => result[1] < double.Epsilon
                ? throw new DivideByZeroException(MathErrorMessager.DivisionByZero)
                : result[0] / result[1]
        };
    }
    
    private static async Task<double> Visit(ConstantExpression node)
    {
        return await Task.FromResult((double) node.Value!);
    }

    private static async Task<double> Visit(UnaryExpression node)
    {
        var val = await VisitExpression(node.Operand);
        return await Task.FromResult(-val);
    }
}