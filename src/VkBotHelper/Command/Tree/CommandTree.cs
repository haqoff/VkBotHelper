using System.Collections.Generic;
using VkBotHelper.Collections;

namespace VkBotHelper.Command.Tree
{
    /// <summary>
    /// Представляет собой дерево токенов шаблонов команд.
    /// </summary>
    public class CommandTree
    {
        internal readonly CommandTreeNode StartNode;

        // сравнение ключа только по ссылке
        private readonly Dictionary<CommandTreeNode, RunMetadata> _globalCommands;
        private readonly MultiValueDictionary<CommandTreeNode, RunMetadata> _contextCommands;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CommandTree"/> с указанным начальным узлом.
        /// </summary>
        internal CommandTree(CommandTreeNode startNode)
        {
            StartNode = startNode;

            _globalCommands = new Dictionary<CommandTreeNode, RunMetadata>();
            _contextCommands = new MultiValueDictionary<CommandTreeNode, RunMetadata>(new RunMetadataByGroupComparer());
        }

        /// <summary>
        /// Связывает указанный узел с метаданными запуска команды.
        /// </summary>
        /// <param name="endNode">Узел, на котором команда завершается.</param>
        /// <param name="metadata"> Метаданные запуска.</param>
        internal void AddMetadata(CommandTreeNode endNode, RunMetadata metadata)
        {
            if (metadata.IsGlobal)
            {
                _globalCommands.Add(endNode, metadata);
            }
            else
            {
                _contextCommands.Add(endNode, metadata);
            }
        }

        /// <summary>
        /// Получает метаданные запуска по указанному узлу глобального типа.
        /// </summary>
        internal RunMetadata GetGlobalMetadata(CommandTreeNode endNode)
        {
            return _globalCommands.GetValueOrDefault(endNode, null);
        }

        /// <summary>
        /// Получает метаданные запуска по указанному узлу контекстного типа.
        /// </summary>
        internal RunMetadata GetContextMetadata(CommandTreeNode endNode, RunMetadata executedCommand)
        {
            return _contextCommands.TryGetActualValue(endNode, executedCommand, out var context)
                ? context
                : null;
        }
    }
}