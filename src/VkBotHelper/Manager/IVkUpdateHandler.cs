using VkNet.Model.GroupUpdate;

namespace VkBotHelper.Manager
{
    public interface IVkUpdateHandler
    {
        /// <summary>
        /// Обрабатывает новое обновление в группе.
        /// </summary>
        /// <param name="update">Обновление.</param>
        void HandleUpdate(GroupUpdate update);
    }
}