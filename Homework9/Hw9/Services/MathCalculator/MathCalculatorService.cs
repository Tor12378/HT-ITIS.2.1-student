using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using Hw9.Dto;
using Hw9.ErrorMessages;
using Hw9.Services.ExpressionParser;
using Hw9.Services.Validator;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    private readonly IValidator _validator;
    private readonly IParser _parser;

    public MathCalculatorService(IValidator validator,  IParser parser)
    {
        _validator = validator;
        _parser = parser;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var error = _validator.ValidateExpression(expression);
        if (error != string.Empty)
        {
            return new CalculationMathExpressionResultDto(error);
        }
        
        var parsedTokens = _parser.Parse(expression!);
        var exp = _parser.ParseTokens(parsedTokens);

        var expVisitor = new MathVisitor();
        var executeBefore = expVisitor.GetExecute(exp);

        var result = await CalculateAsync(exp, executeBefore);
        return double.IsNaN(result) 
            ? new CalculationMathExpressionResultDto(MathErrorMessager.DivisionByZero)
            : new CalculationMathExpressionResultDto(result);
    }
    
    private async Task<double> CalculateAsync(Expression current, Dictionary<Expression, Tuple<Expression, Expression?>> executeBefore)
    {
        if (!executeBefore.ContainsKey(current))
            return double.Parse(current.ToString(), CultureInfo.InvariantCulture);
        
        if (executeBefore[current].Item2 == null)
            return -1 * await CalculateAsync(executeBefore[current].Item1, executeBefore);
        
        var leftTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalculateAsync(executeBefore[current].Item1, executeBefore);
        });
        var rightTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalculateAsync(executeBefore[current].Item2!, executeBefore);
        });

        var result = await Task.WhenAll(leftTask, rightTask);
        return Calculate(result[0], current.NodeType, result[1]);

    }

    [ExcludeFromCodeCoverage]
    private static double Calculate(double arg1, ExpressionType type, double arg2)
    {
        return type switch
        {
            ExpressionType.Add => arg1 + arg2,
            ExpressionType.Subtract => arg1 - arg2,
            ExpressionType.Multiply => arg1 * arg2,
            ExpressionType.Divide when Math.Abs(arg2) < double.Epsilon => double.NaN,
            ExpressionType.Divide => arg1 / arg2,
            _ => throw new InvalidOperationException("That expression type isn't supported")
        };
    }
}