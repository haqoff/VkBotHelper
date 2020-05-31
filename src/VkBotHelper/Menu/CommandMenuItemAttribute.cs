using System;
using VkBotHelper.Command;

namespace VkBotHelper.Menu
{
    /// <summary>
    /// Представляет собой атрибут для задания значения выбора в меню.
    /// </summary>
    public class CommandMenuItemAttribute : CommandAttribute
    {
        public readonly string DisplayText;

        /// <summary>
        /// Помечает указанный метод как команду меню.
        /// </summary>
        /// <param name="pattern">Шаблон сообщения, по которому будет срабатывать данный метод.</param>
        /// <param name="displayText">Отображаемый текст в меню.</param>
        public CommandMenuItemAttribute(string pattern, string displayText) : base(pattern, false)
        {
            DisplayText = displayText ?? throw new ArgumentNullException(nameof(displayText));
        }
    }
}