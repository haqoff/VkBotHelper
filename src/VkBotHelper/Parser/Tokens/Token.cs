using System.Diagnostics;

namespace VkBotHelper.Parser.Tokens
{
    /// <summary>
    /// Представляет собой лексему.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Type) + "}")]
    public class Token
    {
        /// <summary>
        /// Тип токена.
        /// </summary>
        public readonly TokenType Type;

        public Token(TokenType type)
        {
            Type = type;
        }

        protected bool Equals(Token other)
        {
            return Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            return (int) Type;
        }
    }
}
