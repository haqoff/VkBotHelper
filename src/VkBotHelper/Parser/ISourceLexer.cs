using VkBotHelper.Parser.Tokens;

namespace VkBotHelper.Parser
{
    public interface ISourceLexer
    {
        Token NextSourceToken();
    }
}
