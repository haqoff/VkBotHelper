using System.Collections;
using System.Collections.Generic;

namespace VkBotHelper.Parser.Tokens.Values.Containers
{
    /// <summary>
    /// Представляет собой группу-контейнер, который содержит список повторяющихся контейнеров.
    /// </summary>
    public class GroupContainer : GenericTokenWithValueContainer<ValueContainer>, IReadOnlyList<ValueContainer>
    {
        // ReSharper disable once CoVariantArrayConversion
        internal GroupContainer(TokenWithValue<ValueContainer>[] iterations) : base(iterations)
        {
        }

        /// <summary>
        /// Создаёт токен, хранящий группу с повторениями.
        /// </summary>
        public static TokenWithValue<GroupContainer> CreateToken(params TokenWithValue<ValueContainer>[] iterations)
        {
            return new TokenWithValue<GroupContainer>(new GroupContainer(iterations), TokenType.GroupContainer);
        }

        public IEnumerator<ValueContainer> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                var con = Get<ValueContainer>(i);
                yield return con;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ValueContainer this[int index] => Get<ValueContainer>(index);
    }
}