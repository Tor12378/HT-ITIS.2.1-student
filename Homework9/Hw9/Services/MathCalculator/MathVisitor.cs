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
    
    protected override Expression VisitBinary(BinaryExpression node)
    {
        _execute.Add(node, new Tuple<Expression, Expression?>(node.Left, node.Right));
        return base.VisitBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        _execute.Add(node, new Tuple<Expression, Expression?>(node.Operand, null));
        return base.VisitUnary(node);
    }
}