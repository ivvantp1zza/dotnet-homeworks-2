using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw10.Dto;
using Hw10.Services.Visitor;

namespace Hw10.Services.MathCalculator;

[ExcludeFromCodeCoverage]
public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            var expr = ExpressionParser.ExpressionParser.Parse(expression);
            var visitor = new MyExpressionVisitor();
            var result = Expression.Lambda<Func<double>>(await visitor.MyVisit(expr)).Compile().Invoke();
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
}