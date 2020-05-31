using System.Collections.Generic;
using System.Diagnostics;
using VkBotHelper.Exceptions;
using VkBotHelper.Parser;
using VkBotHelper.Parser.Tokens;
using VkBotHelper.Parser.Tokens.Values.Containers;

namespace VkBotHelper.Command.Tree
{
    /// <summary>
    /// Предоставляет методы для работы с префиксным деревом токенов.
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// Создаёт последовательность узлов в дереве c помощью указанного лексера шаблонов команд.
        /// </summary>
        /// <param name="lexer">Лексер.</param>
        /// <param name="startNode">Стартовый узел дерева.</param>
        /// <returns>Последний узел созданной последовательности типа <see cref="TokenType.EndOfText"/>.</returns>
        internal static CommandTreeNode CreateTreeNodes(IPatternLexer lexer, CommandTreeNode startNode)
        {
            Debug.Assert(lexer != null);
            Debug.Assert(startNode != null);

            var curPointer = startNode;
            var unprocessedOpenParen = 0;
            var stackAfterOpenParen = new Stack<CommandTreeNode>();

            while (curPointer.TokenPrototype.Type != TokenType.EndOfText)
            {
                var token = lexer.NextPatternToken();
                if (token.Type == TokenType.OpenParen)
                {
                    unprocessedOpenParen++;
                }
                else if (token.Type == TokenType.CloseParen)
                {
                    if (stackAfterOpenParen.Count == 0)
                        throw new WrongPatternException(token,
                            "Количество закрывающих скобок превышает количество открывающих.");

                    if (unprocessedOpenParen > 0)
                        throw new WrongPatternException(token,
                            "Группа не может быть пустой (внутри скобок должны быть значимые токены).");

                    var startAfterNode = stackAfterOpenParen.Pop();
                    var endNode = curPointer;
                    endNode.IsGroupEnd = true;

                    var actionPeek = lexer.NextPatternToken();

                    if (actionPeek.Type == TokenType.Plus)
                    {
                        // создаём цикличную ссылку, тем самым, группа может повторяться.

                        if (endNode.NextNodes.ContainsKey(startAfterNode.TokenPrototype))
                        {
                            throw new WrongPatternException(actionPeek, "Группа не может содержать только одну подгруппу.");
                        }

                        var backReference = new CommandTreeNode.Reference(startAfterNode, true);
                        endNode.NextNodes.Add(startAfterNode.TokenPrototype, backReference);
                    }
                    else
                    {
                        throw new WrongPatternException(actionPeek,
                            $"После закрывающей скобки может быть только квантификатор '+' - 1 или более раз. Но был встречен токен: {actionPeek}.");
                    }
                }
                else
                {
                    if (curPointer.NextNodes.TryGetValue(token, out CommandTreeNode.Reference reference))
                    {
                        curPointer = reference.Node;
                    }
                    else
                    {
                        var newNode = new CommandTreeNode(token) {InGroupStartCount = unprocessedOpenParen};

                        var newReference = new CommandTreeNode.Reference(newNode, false);

                        while (unprocessedOpenParen > 0)
                        {
                            stackAfterOpenParen.Push(newNode);
                            unprocessedOpenParen--;
                        }

                        curPointer.NextNodes.Add(token, newReference);
                        curPointer = newNode;
                    }
                }
            }

            if (stackAfterOpenParen.Count != 0)
            {
                throw new WrongPatternException(stackAfterOpenParen.Pop().TokenPrototype,
                    "Количество открывающих скобок превышает количество закрывающих.");
            }

            Debug.Assert(curPointer.NextNodes.Count == 0);
            return curPointer;
        }

        /// <summary>
        /// Совершает обход префиксного дерева токенов с помощью указанного лексера.
        /// </summary>
        /// <param name="lexer">Лексер исходного текста.</param>
        /// <param name="start">Стартовый узел дерева.</param>
        /// <returns>Последний узел и контейнер значений. Если обход закончен неудачно, контейнер значений будет являться <see langword="null"/>.</returns>
        internal static (CommandTreeNode endNode, ValueContainer container) Traverse(ISourceLexer lexer, CommandTreeNode start)
        {
            var currentNode = start;
            var stack = new Stack<int>();
            var values = new List<object>();

            var isSequenceRepeat = false;

            while (true)
            {
                var token = lexer.NextSourceToken();
                if (!currentNode.NextNodes.TryGetValue(token, out var reference))
                {
                    break;
                }

                currentNode = reference.Node;

                if (token.Type == TokenType.EndOfText)
                {
                    return (currentNode, new ValueContainer(values.ToArray()));
                }

                if (reference.IsBack)
                {
                    isSequenceRepeat = true;
                }

                var startCount = currentNode.InGroupStartCount;
                if (isSequenceRepeat)
                {
                    if (startCount > 0)
                        stack.Push(values.Count);
                }
                else
                {
                    while (startCount > 0)
                    {
                        stack.Push(values.Count);
                        startCount--;
                    }
                }

                if (TokenFacts.IsPlaceholder(currentNode.TokenPrototype.Type))
                {
                    values.Add(token);
                }

                if (currentNode.IsGroupEnd)
                {
                    Debug.Assert(stack.Count > 0);

                    var index = stack.Pop();
                    var newContainerToken = ReduceToIterationContainer(values, index);

                    if (isSequenceRepeat)
                    {
                        var groupContainerToken = values[^1] as TokenWithValue<GroupContainer>;
                        Debug.Assert(groupContainerToken != null);

                        groupContainerToken.Value.AddToken(newContainerToken);

                        isSequenceRepeat = false;
                    }
                    else
                    {
                        var groupContainerToken = GroupContainer.CreateToken(newContainerToken);

                        values.Add(item: groupContainerToken);
                    }
                }
            }

            return (currentNode, null);
        }

        private static TokenWithValue<ValueContainer> ReduceToIterationContainer(List<object> values, int indexFrom)
        {
            var length = values.Count - indexFrom;
            var items = new object[length];

            values.CopyTo(indexFrom, items, 0, length);
            values.RemoveRange(indexFrom, length);

            var newContainerToken = ValueContainer.CreateToken(items);

            return newContainerToken;
        }
    }
}