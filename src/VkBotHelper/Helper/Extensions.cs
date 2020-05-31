using Unity;
using Unity.Resolution;
using VkBotHelper.Command;
using VkBotHelper.Manager;

namespace VkBotHelper.Helper
{
    public static class Extensions
    {
        /// <summary>
        /// Создает указанный тип команды и устанавливает его активным.
        /// </summary>
        /// <typeparam name="T">Тип команды.</typeparam>
        /// <param name="container">Контейнер зависимостей, в котором должны быть зарегистрированы <see cref="ICommandManager"/>, а также типы для создания команды.</param>
        /// <param name="peerId">Идентификатор чата, в котором необходимо установить команду.</param>
        /// <param name="overrides">Параметры для переопределения конструктора.</param>
        /// <returns></returns>
        public static T CreateCommandAndSetActive<T>(this IUnityContainer container, long peerId,
            params ResolverOverride[] overrides)
        {
            var commandInstance = container.Resolve<T>(overrides);
            var commandManager = container.Resolve<ICommandManager>();
            var metadata = RunMetadata.CreateWithoutDelegate<T>();

            commandManager.SetActiveCommand(peerId, metadata, commandInstance);

            return commandInstance;
        }
    }
}