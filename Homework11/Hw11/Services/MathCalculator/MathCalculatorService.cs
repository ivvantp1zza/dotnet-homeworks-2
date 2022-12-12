using System.Linq.Expressions;
using Hw11.Dto;
using Hw11.Services.Visitor;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        var expr = ExpressionParser.ExpressionParser.Parse(expression);
        return await MyExpressionVisitor.VisitExpression((dynamic)expr);
    }
}