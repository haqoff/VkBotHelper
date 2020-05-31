using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity;
using VkBotHelper.Command;
using VkBotHelper.Command.Tree;
using VkBotHelper.Parser;
using VkBotHelper.Parser.Tokens;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.GroupUpdate;

namespace VkBotHelper.Manager
{
    public class CommandManager : ICommandManager
    {
        private readonly IUnityContainer _container;
        private readonly CommandTree _tree;
        private readonly ConcurrentDictionary<long, (RunMetadata runMetadata, object commandInstance)> _activeCommands;

        /// <summary>
        /// Определяет, может ли являться первый символ сообщения началом команды. Используется в целях оптимизации до работы лексического анализа.
        /// По стандарту включены только '.' '/' '!' и цифры
        /// </summary>
        [CanBeNull] public Func<char, bool> IsCommandStartCharacter = TokenFacts.IsPossibleCommandStartChar;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="CommandManager"/> с заданным контейнером зависимостей и деревом команд.
        /// </summary>
        /// <param name="container">Контейнер зависимостей, в котором должны быть зарегистрированы все используемые типы в конструкторах классов команд.</param>
        /// <param name="tree">Дерево токенов команд.</param>
        public CommandManager(IUnityContainer container, CommandTree tree)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _tree = tree ?? throw new ArgumentNullException(nameof(tree));
            _activeCommands = new ConcurrentDictionary<long, (RunMetadata runMetadata, object commandInstance)>();
        }

        /// <summary>
        /// Обрабатывает указанное обновление на предмет нового сообщения-команды.
        /// </summary>
        public async void HandleUpdate(GroupUpdate update)
        {
            if (update.Type == GroupUpdateType.MessageNew)
                await HandleMessageAsync(update.Message);
        }

        public async Task HandleMessageAsync(Message source)
        {
            // optimization
            if (IsCommandStartCharacter != null && source.Text.Length != 0 && !IsCommandStartCharacter(source.Text[0]))
                return;

            await Task.Run(async () =>
            {
                Debug.Assert(source.PeerId.HasValue);

                var reader = new TextReader(source.Text);
                var lexer = new Lexer(reader);

                var (endNode, container) = TreeHelper.Traverse(lexer, _tree.StartNode);
                if (container == null) return;

                if (_activeCommands.TryGetValue(source.PeerId.Value, out var activeInfo))
                {
                    // в этом чате уже имеется активная команда, ищем контекстную для запуска
                    var contextMetadata = _tree.GetContextMetadata(endNode, activeInfo.runMetadata);
                    if (contextMetadata?.DelegateGetter != null)
                    {
                        var args = new CommandArgs(source, container, contextMetadata);

                        var action = contextMetadata.DelegateGetter(activeInfo.commandInstance);
                        await action(args);
                    }
                }
                else
                {
                    var globalMetadata = _tree.GetGlobalMetadata(endNode);
                    if (globalMetadata?.DelegateGetter != null)
                    {
                        var args = new CommandArgs(source, container, globalMetadata);
                        var instance = _container.Resolve(globalMetadata.CommandClassType);

                        var action = globalMetadata.DelegateGetter(instance);
                        await action(args);
                    }
                }
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public void SetActiveCommand(long peerId, RunMetadata runMetadata, object instance)
        {
            if (runMetadata == null) throw new ArgumentNullException(nameof(runMetadata));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (instance.GetType() != runMetadata.CommandClassType)
                throw new ArgumentException("Тип объекта и тип метаданных не совпадает", nameof(instance));

            _activeCommands.TryAdd(peerId, (runMetadata, instance));
        }

        /// <inheritdoc/>
        public (RunMetadata runMetadata, object commandInstance)? GetActiveCommand(long peerId)
        {
            if (_activeCommands.TryGetValue(peerId, out var activeInfo))
                return activeInfo;

            return null;
        }

        /// <inheritdoc/>
        public bool RemoveActiveCommand(long peerId)
        {
            return _activeCommands.TryRemove(peerId, out _);
        }
    }
}