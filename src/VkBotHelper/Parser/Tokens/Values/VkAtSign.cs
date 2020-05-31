using System.Diagnostics;

namespace VkBotHelper.Parser.Tokens.Values
{
    /// <summary>
    /// Представляет собой обращение @ ВКонтакте.
    /// </summary>
    public struct VkAtSign
    {
        /// <summary>
        /// Идентификатор сообщества/человека.
        /// </summary>
        public readonly long Id;

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// Признак того, что обращение идёт к сообществу. Если равен <see langword="false"/>, то это обращение к человеку.
        /// </summary>
        public readonly bool IsClub;

        public VkAtSign(long id, string displayName, bool isClub)
        {
            Id = id;
            DisplayName = displayName;
            IsClub = isClub;
        }

        public bool Equals(VkAtSign other)
        {
            return Id == other.Id && DisplayName == other.DisplayName && IsClub == other.IsClub;
        }

        public override bool Equals(object obj)
        {
            return obj is VkAtSign other && Equals(other);
        }

        public static bool operator ==(VkAtSign left, VkAtSign right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VkAtSign left, VkAtSign right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsClub.GetHashCode();
                return hashCode;
            }
        }
    }
}