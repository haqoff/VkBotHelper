using System;
using System.Linq;
using System.Threading.Tasks;
using VkBotHelper.Helper;
using VkBotHelper.Manager;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VkBotHelper.Menu
{
    /// <summary>
    /// Представляет собой базовый класс для создания меню.
    /// </summary>
    public abstract class MenuBase
    {
        protected readonly long PeerId;
        private readonly IVkApi _vkApi;
        protected readonly string MenuText;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MenuBase"/>. Для его создания в контейнере зависимостей необходимо зарегистрировать <see cref="IVkApi"/> и <see cref="ICommandManager"/>.
        /// </summary>
        /// <param name="peerId">Идентификатор чата, в котором работает меню.</param>
        /// <param name="vkApi">API-ВКонтакте</param>
        protected MenuBase(long peerId, IVkApi vkApi)
        {
            PeerId = peerId;
            _vkApi = vkApi;
            MenuText = string.Join('\n',
                CommonHelper.ExtractMethodsWithAttribute<CommandMenuItemAttribute>(GetType()).Select(p => p.Key.DisplayText));
        }

        /// <summary>
        /// Синхронно отсылает сообщение, содержащее список меню.
        /// </summary>
        protected void DisplayMenuText()
        {
            _vkApi.Messages.Send(new MessagesSendParams() {PeerId = PeerId, Message = MenuText, RandomId = new Random().Next()});
        }

        /// <summary>
        /// Асинхронно отсылает сообщение, содержащее список меню.
        /// </summary>
        /// <returns></returns>
        protected async Task DisplayMenuTextAsync()
        {
            await _vkApi.Messages.SendAsync(new MessagesSendParams()
                {PeerId = PeerId, Message = MenuText, RandomId = new Random().Next()});
        }
    }
}