using System.Globalization;
namespace Hw8.Calculator;

public class Parser
{
    public static (double, Operation, double) ParseArguments(string val1, string op, string val2)
    {
        if (!(Double.TryParse(val1, NumberStyles.Any, CultureInfo.InvariantCulture, out var value1) &&
              Double.TryParse(val2, NumberStyles.Any, CultureInfo.InvariantCulture, out var value2)))
            throw new ArgumentException("Incorrect arguments for calculation");
        
        var operation = ParseOperation(op);
        if (operation == Operation.Invalid)
            throw new InvalidOperationException("Incorrect operation");
        
        return (value1, operation, value2);
    }

    private static Operation ParseOperation(string op)
    {
        return op switch
        {
            "Plus" => Operation.Plus,
            "Minus" => Operation.Minus,
            "Multiply" => Operation.Multiply,
            "Divide" => Operation.Divide,
            _ => Operation.Invalid
        };
    }
}