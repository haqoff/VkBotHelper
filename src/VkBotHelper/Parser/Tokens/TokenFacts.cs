namespace VkBotHelper.Parser.Tokens
{
    /// <summary>
    /// Представляет информацию о типах токенов.
    /// </summary>
    public static class TokenFacts
    {
        /// <summary>
        /// Определяет, является ли указанный тип токена плейсхолдером.
        /// </summary>
        public static bool IsPlaceholder(TokenType type)
        {
            switch (type)
            {
                case TokenType.DatePlaceholder:
                case TokenType.FromTodayOffsetPlaceholder:
                case TokenType.StringLiteralPlaceholder:
                case TokenType.DoubleLiteralPlaceholder:
                case TokenType.TimePlaceholder:
                case TokenType.AtSignPlaceholder:
                case TokenType.DayOfWeekOffsetPlaceholder:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Преобразует указанный тип в исходный тип, используемый при разборе исходного сообщения.
        /// </summary>
        /// <example>
        /// DatePlaceholder => Date
        /// </example>
        public static TokenType ToSourceType(TokenType t)
        {
            return t switch
            {
                TokenType.DatePlaceholder => TokenType.Date,
                TokenType.FromTodayOffsetPlaceholder => TokenType.FromTodayOffset,
                TokenType.StringLiteralPlaceholder => TokenType.StringLiteral,
                TokenType.DoubleLiteralPlaceholder => TokenType.DoubleLiteral,
                TokenType.TimePlaceholder => TokenType.Time,
                TokenType.AtSignPlaceholder => TokenType.AtSign,
                TokenType.DayOfWeekOffsetPlaceholder => TokenType.DayOfWeekOffset,
                _ => t
            };
        }

        /// <summary>
        /// Определяет, может ли являться указанный символ началом команды.
        /// </summary>
        /// <param name="c">Символ.</param>
        public static bool IsPossibleCommandStartChar(char c)
        {
            switch (c)
            {
                case '/':
                case '!':
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                default:
                    return false;
            }
        }
    }
}