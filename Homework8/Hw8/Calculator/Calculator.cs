namespace Hw8.Calculator;

public class Calculator : ICalculator
{
    public double Plus(double val1, double val2) => val1 + val2;

    public double Minus(double val1, double val2) => val1 - val2;

    public double Multiply(double val1, double val2) => val1 * val2;

    public double Divide(double firstValue, double secondValue)
    {
        if (secondValue == 0)
            throw new InvalidOperationException("Division by zero");
        return (firstValue / secondValue);
    }
    
    public double Calculate(Operation operation, double val1, double val2)
    {
        switch (operation)
        {
            case Operation.Plus:
                return Plus(val1, val2);
            case Operation.Minus:
                return Minus(val1, val2);
            case Operation.Multiply:
                return Multiply(val1, val2);
            case Operation.Divide:
                if (val2 == 0)
                    throw new DivideByZeroException("Division by zero");
                return Divide(val1, val2);
            default:
                throw new InvalidOperationException("Invalid operation");
        }
    }
}