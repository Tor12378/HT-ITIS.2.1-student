using System.Globalization;
using System.Linq.Expressions;
using Hw9.Services.TokenParser;
using System.Diagnostics.CodeAnalysis;
namespace Hw9.Services.ExpressionParser;

public class Parser : IParser
{
    readonly char[] _operations = { '+', '-', '*', '/' };
    readonly char[] _brackets = { '(', ')' };
    readonly List<Token> _tokens = new();
    
    public List<Token> Parse(string input)
    {
        var i = 0;
        while (i < input.Length)
        {
            if (_brackets.Contains(input[i]))
                _tokens.Add(ParseBracket(input, i));
            else if (_operations.Contains(input[i]))
                _tokens.Add(ParseOperation(input, i));
            else if (char.IsDigit(input[i]))
                _tokens.Add(ParseNumber(input, ref i));
            
            i++;
        }
        
        return _tokens;
    }

    private Token ParseNumber(string input, ref int position)
    {
        var startPos = position;
        while (position < input.Length && (char.IsDigit(input[position]) || input[position] == '.'))
            position++;
        
        return new Token(TokenType.Number, input[startPos..position--]);
    }
    
    [ExcludeFromCodeCoverage]
    private Token ParseOperation(string input, int pos)
    {
        return input[pos] switch
        {
            '+' => new Token(TokenType.Plus, "+"),
            '*' => new Token(TokenType.Multiply, "*"),
            '/' => new Token(TokenType.Divide, "/"),
            '-' when pos == 0 || _tokens[^1].Type == TokenType.OpenBracket 
                => new Token(TokenType.Negate, "-"),
            '-' => new Token(TokenType.Minus, "-"),
            _ => throw new ArgumentException("Cannot parse operation")
        };
    }
    
    [ExcludeFromCodeCoverage]
    private Token ParseBracket(string input, int pos)
    {
        return input[pos] switch
        {
            '(' => new Token(TokenType.OpenBracket, "("),
            ')' => new Token(TokenType.CloseBracket, ")"),
            _ => throw new ArgumentException("Cannot parse bracket")
        };
    }
    
    public Expression ParseTokens(IEnumerable<Token> tokens)
    {
        var expStack = new Stack<Expression>();
        var stack = new Stack<Token>();
        
        foreach (var token in tokens)
        {
            if (token.IsOperation)
            {
                while (stack.TryPeek(out var last) && last.Priority >= token.Priority)
                {
                        PushOperation(stack.Pop(), expStack);
                }
                stack.Push(token);
                continue;
            }
            
            if (token.Type == TokenType.Number)
            {
                expStack.Push(Expression.Constant(
                    double.Parse(token.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)));
                continue;
            }
            
            if (token.Type == TokenType.OpenBracket)
            {
                stack.Push(token);
                continue;
            }

            if (token.Type == TokenType.CloseBracket)
            {
                while (stack.TryPeek(out var last))
                {
                    if (last.Type != TokenType.OpenBracket)
                        PushOperation(stack.Pop(), expStack);
                    else
                        break;
                }
                stack.Pop();
            }
        }

        while (stack.TryPeek(out var last))
            PushOperation(stack.Pop(), expStack);
        
        return expStack.Pop();
    }
   
    private void PushOperation(Token token, Stack<Expression> stack)
    {
        switch (token.Type)
        {
            case TokenType.Plus:
                stack.Push(Expression.Add(stack.Pop(), stack.Pop()));
                break;
            case TokenType.Minus:
                var second = stack.Pop();
                var first = stack.Pop();
                stack.Push(Expression.Subtract(first, second));
                break;
            case TokenType.Multiply:
                stack.Push(Expression.Multiply(stack.Pop(), stack.Pop()));
                break;
            case TokenType.Divide:
                second = stack.Pop();
                first = stack.Pop();
                stack.Push(Expression.Divide(first, second));
                break;
            case TokenType.Negate:
                stack.Push(Expression.Negate(stack.Pop()));
                break;
        }
    }
}