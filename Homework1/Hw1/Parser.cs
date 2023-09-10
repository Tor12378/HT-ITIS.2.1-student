namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args,
        out double val1,
        out CalculatorOperation operation,
        out double val2)
    {
        if (!IsArgLengthSupported(args))
            throw new ArgumentException("Invalid number of arguments specified");
        val1 = ParseStringToDouble(args[0]);
        operation = ParseOperation(args[1]);
        val2 = ParseStringToDouble(args[2]);
        }

    private static double ParseStringToDouble(string s)
    {
        if (double.TryParse(s, out var number))
        {
            return number;
        }
        throw new ArgumentException("The entered argument is not a number");
    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static CalculatorOperation ParseOperation(string arg)
    {
        return arg switch
        {
            "+" => CalculatorOperation.Plus,
            "-" => CalculatorOperation.Minus,
            "*" => CalculatorOperation.Multiply,
            "/" => CalculatorOperation.Divide,
            _ => throw new InvalidOperationException("Operation specified incorrectly")
        };
    }
}