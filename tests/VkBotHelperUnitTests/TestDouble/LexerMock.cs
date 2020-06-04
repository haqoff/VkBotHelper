using VkBotHelper.Parser;
using VkBotHelper.Parser.Tokens;

namespace VkBotHelperUnitTests.TestDouble
{
    public class LexerMock : ISourceLexer, IPatternLexer
    {
        private readonly Token[] _tokens;
        private int _current;

        public LexerMock(params Token[] tokens)
        {
            _tokens = tokens;
        }

        public Token NextSourceToken()
        {
            return _current < _tokens.Length ? _tokens[_current++] : _tokens[^1];
        }

        public Token NextPatternToken()
        {
            return _current < _tokens.Length ? _tokens[_current++] : _tokens[^1];
        }
    }
}
