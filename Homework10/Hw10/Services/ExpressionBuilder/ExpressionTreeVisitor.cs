using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw10.ErrorMessages;

namespace Hw10.Services.ExpressionBuilder;

[ExcludeFromCodeCoverage]
public class ExpressionTreeVisitor : ExpressionVisitor
{
    public static async Task<double> VisitAsync(Expression expression)
    {

        if (expression is BinaryExpression)
        {
            var binExpr = expression as BinaryExpression;
            await Task.Delay(1000);
            var leftExpr = Task.Run(() => VisitAsync(binExpr!.Left));
            var rightExpr = Task.Run(() => VisitAsync(binExpr!.Right));
            var res = await Task.WhenAll(leftExpr, rightExpr);

            var constLeft = res[0];
            var constRight = res[1];

            return Calculate(binExpr!.NodeType, constLeft, constRight);

        }

        if (expression is ConstantExpression constExp)
            return (double)constExp.Value!;

        throw new NullReferenceException();
    }

    public static double Calculate(ExpressionType binExpr, double constLeft,double constRight)
    {
        return (binExpr) switch
        {
            ExpressionType.Add => constLeft + constRight,
            ExpressionType.Subtract => constLeft - constRight,
            ExpressionType.Multiply => constLeft * constRight,
            ExpressionType.Divide => constRight == 0.0
                ? throw new Exception(MathErrorMessager.DivisionByZero)
                : constLeft / constRight,
            _ => throw new Exception(MathErrorMessager.UnknownCharacter)
        };
    } 
}