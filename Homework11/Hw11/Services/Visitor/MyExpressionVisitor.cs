using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw11.ErrorMessages;

namespace Hw11.Services.Visitor;

[ExcludeFromCodeCoverage]
public class MyExpressionVisitor 
{
    [ExcludeFromCodeCoverage]
    public static async Task<double> VisitExpression(Expression expr)
    {
        return await VisitNode(expr as dynamic);
    }
    
    private static async Task<double> VisitNode(BinaryExpression binExpr)
    {
        await Task.Delay(1000);
        var t = VisitExpression(binExpr.Left);
        var t1 = VisitExpression(binExpr.Right);
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
    
    private static async Task<double> VisitNode(ConstantExpression node)
    {
        return await Task.FromResult((double) node.Value!);
    }
}