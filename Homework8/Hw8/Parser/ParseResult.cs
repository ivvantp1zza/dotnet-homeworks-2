using Hw8.Calculator;

namespace Hw8.Parser;

public class ParseResult
{
    public double Value1 { get; }
    public Operation Operation { get; }
    public double Value2 { get; }
    public string ErrorMessage { get; }

    public ParseResult(double value1, Operation operation, double value2, string error = null)
    {
        Value1 = value1;
        Operation = operation;
        Value2 = value2;
        ErrorMessage = error;
    }
}