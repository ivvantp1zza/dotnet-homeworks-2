using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        [FromQuery] string val1,
        [FromQuery] string operation,
        [FromQuery] string val2)
    {
        var args = new string[] { val1, operation, val2 };
        var parsed = Parser.Parser.ParseCalcArguments(args);
        if (parsed.ErrorMessage != null)
            return BadRequest(parsed.ErrorMessage);
        return Ok(calculator.Calculate(parsed.Value1, parsed.Operation, parsed.Value2));
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}