using System.Collections.Generic;
using VkBotHelper.Parser.Tokens;

namespace VkBotHelper.Command.Tree
{
    /// <summary>
    /// Представляет собой узел префиксного дерева токенов.
    /// </summary>
    internal class CommandTreeNode
    {
        private static readonly SourcePatternTokenComparer Comparer = new SourcePatternTokenComparer();

        /// <summary>
        /// Определяет, во скольких групп данный узел является началом группы (т.е. сколько перед этим токеном в шаблоне было открывающих скобок). 
        /// </summary>
        internal int InGroupStartCount = 0;

        /// <summary>
        /// Определяет, является ли данный узел конечным в какой-либо группе (т.е после этого токена в шаблоне была закрывающая скобка).
        /// </summary>
        internal bool IsGroupEnd = false;

        /// <summary>
        /// Прототип токена этого узла.
        /// </summary>
        internal readonly Token TokenPrototype;

        /// <summary>
        /// Дочерние узлы.
        /// </summary>
        internal readonly Dictionary<Token, Reference> NextNodes;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CommandTreeNode"/> с указанным прототипом токена.
        /// </summary>
        /// <param name="prototype">Токен, представляющий данный узел.</param>
        public CommandTreeNode(Token prototype)
        {
            TokenPrototype = prototype;
            NextNodes = new Dictionary<Token, Reference>(Comparer);
        }

        /// <summary>
        /// Представляет собой ссылку на узел.
        /// </summary>
        internal class Reference
        {
            /// <summary>
            /// Узел.
            /// </summary>
            internal readonly CommandTreeNode Node;

            /// <summary>
            /// Признак того, что ссылка указывает назад в дереве.
            /// </summary>
            internal readonly bool IsBack;

            internal Reference(CommandTreeNode node, bool isBack)
            {
                Node = node;
                IsBack = isBack;
            }
        }
    }
}