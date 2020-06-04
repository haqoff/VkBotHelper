using System;
using Unity;
using Unity.Lifetime;
using VkBotHelper.Command.Tree;
using VkBotHelper.Manager;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBotHelper.Helper
{
    public static class VkBot
    {
        /// <summary>
        /// Создаёт и запускает бота команд.
        /// </summary>
        /// <param name="botToken">Токен группы бота.</param>
        /// <param name="groupId">Идентификатор группы.</param>
        /// <param name="commandTreeAction">Действие-делегат, которое предназначено для регистрации команд.</param>
        /// <param name="unityBuilder">Действие-делегат, с помощью которого регистрируются зависимости, необходимые в классах команд.
        /// По-умолчанию зарегистрированы <see cref="IVkApi"/>, <see cref="IVkUpdateListener"/>, <see cref="ICommandManager"/>.</param>
        public static void StartNewCommandBot(string botToken, ulong groupId, Action<CommandTreeBuilder> commandTreeAction,
            Action<IUnityContainer> unityBuilder = null)
        {
            var vkApi = new VkApi();
            vkApi.Authorize(new ApiAuthParams() {AccessToken = botToken});

            var treeBuilder = new CommandTreeBuilder();
            commandTreeAction(treeBuilder);

            var unity = new UnityContainer();
            unityBuilder?.Invoke(unity);

            var commandManager = new CommandManager(unity, treeBuilder.Build());
            var updateListener = new VkUpdateListener(vkApi, groupId);

            unity.RegisterInstance<IVkApi>(vkApi, new SingletonLifetimeManager());
            unity.RegisterInstance<IVkUpdateListener>(updateListener, new SingletonLifetimeManager());
            unity.RegisterInstance<ICommandManager>(commandManager, new SingletonLifetimeManager());

            updateListener.OnUpdateHappened += commandManager.HandleUpdate;
            updateListener.Start();
        }
    }
}