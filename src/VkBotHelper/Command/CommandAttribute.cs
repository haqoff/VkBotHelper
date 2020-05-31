using System;

namespace VkBotHelper.Command
{
    /// <summary>
    /// Представляет собой атрибут для задания метода-команды.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public readonly string Pattern;
        public readonly bool IsGlobal;

        /// <summary>
        /// Помечает метод, как команду для запуска с указанным шаблоном и типом.
        /// </summary>
        /// <param name="pattern">Шаблон текста, при котором будет запускаться данный метод.</param>
        /// <param name="isGlobal">
        /// Признак того, что команда является глобальной.
        /// Глобальная команда может запускаться только тогда, когда в чате или беседе нет активной команды.
        /// Контекстная команда может запускаться только тогда, когда в чате или беседе уже установлена активная команда из этого же класса.
        /// </param>
        public CommandAttribute(string pattern, bool isGlobal)
        {
            Pattern = pattern;
            IsGlobal = isGlobal;
        }
    }
}