namespace VkBotHelper.Parser.Tokens.Values.Containers
{
    /// <summary>
    /// Представляет собой контейнер, хранящий результат разбора.
    /// </summary>
    public class ValueContainer : GenericTokenWithValueContainer<object>
    {
        internal ValueContainer(object[] items) : base(items)
        {
        }

        internal static TokenWithValue<ValueContainer> CreateToken(object[] items)
        {
            return new TokenWithValue<ValueContainer>(new ValueContainer(items), TokenType.OneRepeatContainer);
        }
    }
}