using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VkBotHelper.Exceptions;
using VkBotHelper.Helper;
using VkBotHelper.Menu;
using VkBotHelper.Parser;
using VkBotHelper.Parser.Tokens;

#pragma warning disable 8509

namespace VkBotHelper.Command.Tree
{
    /// <summary>
    /// Представляет собой строитель префиксного дерева команд.
    /// </summary>
    public class CommandTreeBuilder
    {
        private readonly CommandTree _tree;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CommandTreeBuilder"/>.
        /// </summary>
        public CommandTreeBuilder()
        {
            var startNode = new CommandTreeNode(new Token(TokenType.Unknown));
            _tree = new CommandTree(startNode);
        }

        /// <summary>
        /// Извлекает из указанного класса <typeparamref name="T"/> команды с помощью атрибута <see cref="CommandAttribute"/>. 
        /// </summary>
        /// <typeparam name="T">Тип класса.</typeparam>
        /// <returns>Шаблон команды и метаданные запуска команды.</returns>
        internal static IEnumerable<(string pattern, RunMetadata metadata)> ExtractCommandsFromClass<T>()
        {
            var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var attrType = typeof(CommandAttribute);
            var taskType = typeof(Task);
            var expectedMethodParameterType = typeof(CommandArgs);

            foreach (var methodInfo in methods)
            {
                var attribute = (CommandAttribute) Attribute.GetCustomAttribute(methodInfo, attrType);
                if (attribute == null) continue;

                var methodParameters = methodInfo.GetParameters();

                var isGlobal = attribute.IsGlobal;
                var isAsync = methodInfo.ReturnType == taskType;
                var hasArg = methodParameters.Length switch
                {
                    0 => false,
                    1 when methodParameters[0].ParameterType == expectedMethodParameterType => true,
                    _ => throw new VkBotHelperException(
                        "Метод, помеченный атрибутом Command, должен либо не иметь параметров, либо иметь единственный параметр с типом CommandArgs.")
                };

                var metadata = (isAsync, hasArg) switch
                {
                    (false, false) => RunMetadata.Create<T>(isGlobal,
                        target => methodInfo.CreateTypedDelegate<Action>(target)),
                    (true, true) => RunMetadata.CreateWithArgAndAsync<T>(isGlobal,
                        target => methodInfo.CreateTypedDelegate<Func<CommandArgs, Task>>(target)),
                    (true, false) => RunMetadata.CreateWithAsync<T>(isGlobal,
                        target => methodInfo.CreateTypedDelegate<Func<Task>>(target)),
                    (false, true) => RunMetadata.CreateWithArg<T>(isGlobal,
                        target => methodInfo.CreateTypedDelegate<Action<CommandArgs>>(target))
                };

                yield return (attribute.Pattern, metadata);
            }
        }

        /// <summary>
        /// Регистрирует указанный тип, в котором содержится набор команд-методов помеченных атрибутом <see cref="CommandAttribute"/> или <see cref="CommandMenuItemAttribute"/>.
        /// </summary>
        public CommandTreeBuilder Register<T>()
        {
            foreach (var (pattern, metadata) in ExtractCommandsFromClass<T>())
            {
                var lexer = new Lexer(new TextReader(pattern));
                Register(lexer, metadata);
            }

            return this;
        }

        /// <summary>
        /// Регистрирует новую команду с помощью указанных лексера для разбора шаблона и метаданных запуска команды.
        /// </summary>
        /// <param name="lexer">Лексический анализатор, предназначаемый для разбора шаблона.</param>
        /// <param name="runMetadata">Метаданные запуска команды.</param>
        public CommandTreeBuilder Register(IPatternLexer lexer, RunMetadata runMetadata)
        {
            if (lexer == null) throw new ArgumentNullException(nameof(lexer));
            if (runMetadata == null) throw new ArgumentNullException(nameof(runMetadata));

            var endNode = TreeHelper.CreateTreeNodes(lexer, _tree.StartNode);
            _tree.AddMetadata(endNode, runMetadata);

            return this;
        }

        /// <summary>
        /// Получает созданное дерево.
        /// </summary>
        public CommandTree Build() => _tree;
    }
}