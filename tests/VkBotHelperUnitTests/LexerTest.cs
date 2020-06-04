using System;
using System.Linq;
using VkBotHelper.Parser;
using VkBotHelper.Parser.Tokens;
using VkBotHelper.Parser.Tokens.Values;
using Xunit;

namespace VkBotHelperUnitTests
{
    public class LexerTest
    {
        [Fact]
        public void TestSourceValidFromTodayOffsetPlaceholder()
        {
            var values = new[]
            {
                ("завтра", 1), ("послезавтра", 2), ("послепослезавтра", 3),
                ("вчера", -1), ("позавчера", -2), ("позапозавчера", -3)
            };

            var lexer = new Lexer(new TextReader(string.Join(' ', values.Select(v => v.Item1))));
            for (var i = 0; i < values.Length; i++)
            {
                var next = lexer.NextSourceToken();

                Assert.Equal(TokenType.FromTodayOffset, next.Type);
                Assert.IsType<TokenWithValue<int>>(next);

                var t = (TokenWithValue<int>) next;
                Assert.Equal(values[i].Item2, t.Value);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }

        [Fact]
        public void TestSourceValidDayOfWeekOffsetPlaceholder()
        {
            var values = new[]
            {
                ("пн", 0), ("вт", 1), ("ср", 2), ("чт", 3), ("пт", 4), ("сб", 5), ("вс", 6),
                ("понедельник", 0), ("вторник", 1), ("среда", 2), ("четверг", 3), ("пятница", 4), ("суббота", 5),
                ("воскресенье", 6),

                ("след понедельник", 7),
                ("следующий понедельник", 7),

                ("пред понедельник", -7),
                ("предыдущий понедельник", -7)
            };

            var lexer = new Lexer(new TextReader(string.Join(' ', values.Select(v => v.Item1))));
            for (var i = 0; i < values.Length; i++)
            {
                var next = lexer.NextSourceToken();

                Assert.Equal(TokenType.DayOfWeekOffset, next.Type);
                Assert.IsType<TokenWithValue<int>>(next);

                var t = (TokenWithValue<int>) next;
                Assert.Equal(values[i].Item2, t.Value);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }

        [Fact]
        public void TestSourceValidAtSignPlaceholder()
        {
            var values = new[] {"[id228|привет]", "[club6|assad]"};

            var lexer = new Lexer(new TextReader(string.Join(' ', values)));
            for (var i = 0; i < values.Length; i++)
            {
                var next = lexer.NextSourceToken();

                Assert.Equal(TokenType.AtSign, next.Type);
                Assert.IsType<TokenWithValue<VkAtSign>>(next);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }

        [Fact]
        public void TestSourceValidStringPlaceholder()
        {
            var values = new[] {"\"test1\"", "'test2'", "«test3»"};

            var lexer = new Lexer(new TextReader(string.Join(' ', values)));
            foreach (var v in values)
            {
                var next = lexer.NextSourceToken();

                Assert.Equal(TokenType.StringLiteral, next.Type);
                Assert.IsType<TokenWithValue<string>>(next);

                var t = (TokenWithValue<string>) next;
                Assert.Equal(v.Substring(1, v.Length - 2), t.Value);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }

        [Fact]
        public void TestSourceValidTimePlaceholder()
        {
            var values = new[] {"12:30", "0:10", "0:0", "0:01"};

            var lexer = new Lexer(new TextReader(string.Join(' ', values)));
            for (var i = 0; i < values.Length; i++)
            {
                var next = lexer.NextSourceToken();

                Assert.Equal(TokenType.Time, next.Type);
                Assert.IsType<TokenWithValue<TimeSpan>>(next);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }

        [Fact]
        public void TestSourceValidDatePlaceholder()
        {
            var values = new[] {"21.01.2012", "5/06/2019", "6-7-15"};

            var lexer = new Lexer(new TextReader(string.Join(' ', values)));
            for (var i = 0; i < values.Length; i++)
            {
                var next = lexer.NextSourceToken();

                Assert.Equal(TokenType.Date, next.Type);
                Assert.IsType<TokenWithValue<Date>>(next);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }

        [Fact]
        public void TestSourceValidNumberPlaceholder()
        {
            var values = new[] {"15", "-2", "+228", "13", "15", "6,3"};

            var lexer = new Lexer(new TextReader(string.Join(' ', values)));
            for (var i = 0; i < values.Length; i++)
            {
                var next = lexer.NextSourceToken();

                Assert.Equal(TokenType.DoubleLiteral, next.Type);
                Assert.IsType<TokenWithValue<double>>(next);
            }

            Assert.Equal(TokenType.EndOfText, lexer.NextPatternToken().Type);
        }


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