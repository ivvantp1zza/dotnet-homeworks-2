using System.Globalization;
using Hw8.Calculator;

namespace Hw8.Parser;

public static class Parser
{
    public static ParseResult ParseCalcArguments(string[] args)
    {
        if (!double.TryParse(args[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var val1) ||
            !double.TryParse(args[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var val2))
            return new ParseResult(0, Operation.Invalid, 0, Messages.InvalidNumberMessage);
        var operation = ParseOperation(args[1]);
        return operation switch
        {
            Operation.Invalid => new ParseResult(val1, operation, val2, Messages.InvalidOperationMessage),
            Operation.Divide when val2 < double.Epsilon => new ParseResult(0, Operation.Invalid, 0,
                Messages.DivisionByZeroMessage),
            _ => new ParseResult(val1, operation, val2)
        };
    }
    
    private static Operation ParseOperation(string arg)
    {
        return arg.ToLower() switch
        {
            "plus" => Operation.Plus,
            "minus" => Operation.Minus,
            "multiply" => Operation.Multiply,
            "divide" => Operation.Divide,
            _ => Operation.Invalid
        };
    }
}