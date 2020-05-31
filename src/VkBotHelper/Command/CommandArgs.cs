using VkBotHelper.Parser.Tokens.Values.Containers;
using VkNet.Model;

namespace VkBotHelper.Command
{
    /// <summary>
    /// Представляет собой аргумент, передаваемый при запуске команды.
    /// </summary>
    public class CommandArgs
    {
        internal CommandArgs(Message sourceMessage, ValueContainer valueContainer, RunMetadata runMetadata)
        {
            SourceMessage = sourceMessage;
            ValueContainer = valueContainer;
            RunMetadata = runMetadata;
        }

        /// <summary>
        /// Исходное сообщение.
        /// </summary>
        public Message SourceMessage { get; }

        /// <summary>
        /// Контейнер значений.
        /// </summary>
        public ValueContainer ValueContainer { get; }

        /// <summary>
        /// Метаданные, с которыми данная команда была запущена.
        /// </summary>
        public RunMetadata RunMetadata { get; }
    }
}