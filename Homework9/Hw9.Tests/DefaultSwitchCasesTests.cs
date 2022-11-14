using System.Linq.Expressions;
using Hw9.Services.MathCalculator.TokenParser;
using Xunit;

namespace Hw9.Tests;

public class DefaultSwitchCasesTests
{
    [Fact]
    public void TokenOperatorDefaultSwitchCase()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TokenParser.GetOperatorToken("123", 1));
    }

    [Fact]
    public void GetTokenBracketDefaultSwitchCase()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TokenParser.GetBracketToken("123", 1));
    }

    [Fact]
    public void GetOperatorPrecedenceOutOfRange()
    {
        var t = new Token(TokenType.Number, "5");
        Assert.Throws<ArgumentOutOfRangeException>(() => t.GetRank());

    }
}