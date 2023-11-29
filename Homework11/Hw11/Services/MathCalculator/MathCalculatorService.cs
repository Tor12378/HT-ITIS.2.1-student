using System.Text.RegularExpressions;
using Hw11.ExpressionHelper;
namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        ExpressionValidator.Validate(expression);
        var expressionParser = new ExpressionParser();
        var split = new Regex("(?<=[-+*/()])|(?=[-+*/()])");
        var expressionNotation = expressionParser.ParseToPolishNotation(split.Split(expression!));
        var expressionConverted = ExpressionTreeConverter.ConvertToExpressionTree(expressionNotation);
        await Task.Delay(1000);
        var result = await MyExpressionVisitor.VisitExpression(expressionConverted);
        return result;
    }
}