using Hw1;

try
{
    Parser.ParseCalcArguments(args, out double firstValue, out var operation, out var secondValue);
    Console.WriteLine(Calculator.Calculate(firstValue, operation, secondValue));
}
catch (Exception e)
{
    Console.WriteLine($"An error occurred: {e.Message}");
}