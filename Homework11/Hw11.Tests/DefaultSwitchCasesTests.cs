using Hw11.Services.MathCalculator.TokenParser;
using Xunit;

namespace Hw11Tests;

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
    public void GetRankDefaultSwitchCase()
    {
        var t = new Token(TokenType.Number, "5");
        Assert.Throws<ArgumentOutOfRangeException>(() => t.GetRank());

    }
}