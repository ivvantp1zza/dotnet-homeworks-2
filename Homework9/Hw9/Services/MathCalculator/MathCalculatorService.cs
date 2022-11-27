using System.Linq.Expressions;
using Hw9.Dto;
using Hw9.Services.MathCalculator.ExpressionParser;
using Hw9.Services.Visitor;

namespace Hw9.Services.MathCalculator;

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