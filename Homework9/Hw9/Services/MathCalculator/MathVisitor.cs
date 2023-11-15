using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathVisitor : ExpressionVisitor
{
    private readonly Dictionary<Expression, Tuple<Expression, Expression?>> _execute = new();

    public Dictionary<Expression, Tuple<Expression, Expression?>> GetExecute(Expression expression)
    {
        Visit(expression);
        return _execute;
    }
}