using VkBotHelper.Command;

namespace VkBotHelper.Manager
{
    public interface ICommandManager : IVkUpdateHandler
    {
        /// <summary>
        /// Устанавливает активной указанную команду.
        /// </summary>
        /// <param name="peerId">Идентификатор чата.</param>
        /// <param name="runMetadata">Метаданные запуска команды.</param>
        /// <param name="instance">Экземпляр класса обработчика команды.</param>
        void SetActiveCommand(long peerId, RunMetadata runMetadata, object instance);

        /// <summary>
        /// Получает активную команду.
        /// </summary>
        /// <param name="peerId">Идентификатор чата.</param>
        /// <returns>Метаданные запущенной команды и экземпляр класса команды или <see langword="null"/>, если активной команды нет.</returns>
        (RunMetadata runMetadata, object commandInstance)? GetActiveCommand(long peerId);

        /// <summary>
        /// Удаляет активную команду по идентификатору чата.
        /// </summary>
        bool RemoveActiveCommand(long peerId);
    }
}