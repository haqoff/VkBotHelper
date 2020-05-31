using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VkBotHelper.Parser.Tokens
{
    /// <summary>
    /// Представляет собой класс для сравнения токена-прототипа в дереве с токеном, полученным из исходного текста.
    /// </summary>
    internal class SourcePatternTokenComparer : IEqualityComparer<Token>
    {
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public bool Equals(Token x, Token y)
        {
            if (TokenFacts.IsPlaceholder(x.Type) || TokenFacts.IsPlaceholder(y.Type))
            {
                return TokenFacts.ToSourceType(x.Type) == TokenFacts.ToSourceType(y.Type);
            }

            return x.Equals(y);
        }

        public int GetHashCode(Token obj)
        {
            if (obj.Type == TokenType.CommandText)
                return obj.GetHashCode();

            return TokenFacts.ToSourceType(obj.Type).GetHashCode();
        }
    }
}