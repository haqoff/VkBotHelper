using System;
using VkNet.Model.GroupUpdate;

namespace VkBotHelper.Manager
{
    /// <summary>
    /// Предоставляет собой long-poll слушателя событий в группе ВК.
    /// </summary>
    public interface IVkUpdateListener
    {
        /// <summary>
        /// Событие, которое запускается при каждом новом событии в группе ВКонтакте (новое сообщение, новый пост и так далее).
        /// </summary>
        event Action<GroupUpdate> OnUpdateHappened;

        /// <summary>
        /// Запускает слушателя на предмет новых событий в группе. Может переиспользоваться и вызываться после <see cref="Stop"/>.
        /// </summary>
        void Start();

        /// <summary>
        /// Останавливает слушателя.
        /// </summary>
        void Stop();
    }
}