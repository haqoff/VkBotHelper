namespace VkBotHelper.Parser.Tokens
{
    /// <summary>
    /// Представляет собой типы токена.
    /// </summary>
    public enum TokenType
    {
        Unknown,
        OpenParen,
        CloseParen,
        Slash,
        Dot,
        EndOfText,
        CommandText,
        StringLiteral,
        Exclamation,
        Plus,
        Date,
        DoubleLiteral,
        Time,
        FromTodayOffset,
        DayOfWeekOffset,
        AtSign,
        GroupContainer,
        OneRepeatContainer,

        #region Placeholders

        DatePlaceholder,
        FromTodayOffsetPlaceholder,
        StringLiteralPlaceholder,
        DoubleLiteralPlaceholder,
        TimePlaceholder,
        AtSignPlaceholder,
        DayOfWeekOffsetPlaceholder

        #endregion

        
    }
}