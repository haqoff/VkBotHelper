using System.Collections.Generic;
using System.Diagnostics;

namespace VkBotHelper.Parser.Tokens
{
    /// <summary>
    /// Представляет собой токен со значением.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Type: {" + nameof(Type) + "} Value: {" + nameof(Value) + "}")]
    public class TokenWithValue<T> : Token
    {
        /// <summary>
        /// Значение токена.
        /// </summary>
        public T Value { get; }

        public TokenWithValue(T value, TokenType type) : base(type)
        {
            Value = value;
        }

        protected bool Equals(TokenWithValue<T> other)
        {
            return base.Equals(other) && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TokenWithValue<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }
    }
}