using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var ans = _dbContext.SolvingExpressions.Where(e => e.Expression == expression);
		if (ans.Any())
			return new CalculationMathExpressionResultDto(ans.First().Result);
		var c = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (c.IsSuccess)
		{
			_dbContext.Add(new SolvingExpression { Expression = expression, Result = c.Result });
			await _dbContext.SaveChangesAsync();
			return c;
		}
		else
			return c;
	}
}