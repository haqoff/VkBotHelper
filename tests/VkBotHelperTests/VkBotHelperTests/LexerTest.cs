using System.Linq;
using VkBotHelper.Parser;
using VkBotHelper.Parser.Tokens;
using Xunit;

namespace VkBotHelperTests
{
    public class LexerTest
    {
        [Fact]
        public void TestPatternPlaceholderSimple()
        {
            var testPlaceholders = new[]
            {
                ("#дата", TokenType.DatePlaceholder),
                ("#время", TokenType.TimePlaceholder),
                ("#строка", TokenType.StringLiteralPlaceholder),
                ("#число", TokenType.DoubleLiteralPlaceholder),
                ("#обращение", TokenType.AtSignPlaceholder),
                ("#день_смещение", TokenType.FromTodayOffsetPlaceholder),
                ("#день_недели", TokenType.DayOfWeekOffsetPlaceholder)
            };

            var lexer = new Lexer(new TextReader(string.Join(' ', testPlaceholders.Select(p => p.Item1))));
            foreach (var tuple in testPlaceholders)
            {
                var next = lexer.NextPatternToken();
                Assert.Equal(tuple.Item2, next.Type);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }
    }
}