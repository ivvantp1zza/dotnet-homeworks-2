using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly IMemoryCache _cache;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMemoryCache cache, IMathCalculatorService simpleCalculator)
	{
		_cache = cache;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		if (_cache.TryGetValue(expression, out double val))
		{
			return new CalculationMathExpressionResultDto(val);
		}
		var c = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (c.IsSuccess)
		{
			_cache.Set(expression, c.Result);
			return c;
		}
		else
			return c;
	}
}