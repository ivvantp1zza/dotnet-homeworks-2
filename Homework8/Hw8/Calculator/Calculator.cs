﻿namespace Hw8.Calculator;

public class Calculator : ICalculator
{
    public double Plus(double val1, double val2) => val1 + val2;

    public double Minus(double val1, double val2) => val1 - val2;

    public double Multiply(double val1, double val2) => val1 * val2;

    public double Divide(double firstValue, double secondValue)
    {
        if (secondValue < double.Epsilon)
            throw new InvalidOperationException(Messages.DivisionByZeroMessage);
        return firstValue / secondValue;
    }

    public double Calculate(double val1, Operation operation, double val2)
    {
        return operation switch
        {
            Operation.Plus => Plus(val1, val2),
            Operation.Minus => Minus(val1, val2),
            Operation.Multiply => Multiply(val1, val2),
            Operation.Divide => Divide(val1, val2),
            _ => throw new InvalidOperationException(Messages.InvalidOperationMessage)
        };
    }
}