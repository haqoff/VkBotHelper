using System;
using VkNet.Abstractions;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace VkBotHelper.Manager
{
    /// <inheritdoc/>
    public class VkUpdateListener : IVkUpdateListener
    {
        /// <inheritdoc/>
        public event Action<GroupUpdate> OnUpdateHappened;

        private volatile bool _running;
        private readonly IVkApi _api;
        private readonly ulong _botGroupId;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="VkUpdateListener"/> для long-poll прослушивания указанной группы <paramref name="botGroupId"/> с помощью <paramref name="api"/>.
        /// </summary>
        /// <param name="botGroupId">Идентификатор группы.</param>
        /// <param name="api">API-ВКонтакте.</param>
        public VkUpdateListener(IVkApi api, ulong botGroupId)
        {
            _api = api;
            _botGroupId = botGroupId;
        }

        /// <inheritdoc/>
        public void Start()
        {
            _running = true;

            while (_running)
            {
                var serverResponse = _api.Groups.GetLongPollServer(_botGroupId);

                var history = _api.Groups.GetBotsLongPollHistory(
                    new BotsLongPollHistoryParams {Server = serverResponse.Server, Ts = serverResponse.Ts, Key = serverResponse.Key, Wait = 25});

                if (history?.Updates == null) continue;

                foreach (var update in history.Updates)
                {
                    OnUpdateHappened?.Invoke(update);
                }
            }
        }

        /// <inheritdoc/>
        public void Stop()
        {
            _running = false;
        }
    }
}