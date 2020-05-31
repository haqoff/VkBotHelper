using VkBotHelper.Parser.Tokens;

namespace VkBotHelper.Parser
{
    public interface IPatternLexer
    {
        Token NextPatternToken();
    }
}
