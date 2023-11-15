using System.Linq.Expressions;
using Hw9.Services.TokenParser;
namespace Hw9.Services.ExpressionParser;

public interface IParser
{
    public Expression ParseTokens(IEnumerable<Token> tokens);
    
    public List<Token> Parse(string input);
}