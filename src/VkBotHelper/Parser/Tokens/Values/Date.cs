using System;

namespace VkBotHelper.Parser.Tokens.Values
{
    /// <summary>
    /// Представляет собой дату без времени.
    /// </summary>
    public struct Date
    {
        /// <summary>
        /// Число, выражающее день.
        /// </summary>
        public readonly int Day;

        /// <summary>
        /// Число, выражающее месяц от 1 до 12.
        /// </summary>
        public readonly int Month;

        /// <summary>
        /// Число, выражающее год.
        /// Если равно -1, то год считается неуказанным.
        /// </summary>
        public readonly int Year;

        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public DateTime ToDateTime() => new DateTime(Year, Month, Day);
    }
}