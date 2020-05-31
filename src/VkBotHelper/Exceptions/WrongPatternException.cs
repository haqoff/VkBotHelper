using VkBotHelper.Parser.Tokens;

namespace VkBotHelper.Exceptions
{
    /// <summary>
    /// Представляет собой ошибку, которая возникает при неверном шаблоне команды.
    /// </summary>
    public class WrongPatternException : VkBotHelperException
    {
        /// <summary>
        /// Токен, который произвёл ошибку и не ожидался в процессе разбора. 
        /// </summary>
        public Token UnexpectedToken { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WrongPatternException"/> с указанными шаблоном, токеном и сообщением об ошибке.
        /// </summary>
        public WrongPatternException(Token unexpectedToken, string message) : base(message)
        {
            UnexpectedToken = unexpectedToken;
        }
    }
}