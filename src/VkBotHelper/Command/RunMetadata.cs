using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace VkBotHelper.Command
{
    /// <summary>
    /// Представляет собой метаданные запуска команды.
    /// </summary>
    public class RunMetadata
    {
        /// <summary>
        /// Тип, новый экземпляр которого будет создаваться для получения метода запуска.
        /// </summary>
        [NotNull] public readonly Type CommandClassType;

        /// <summary>
        /// Функция, которая возвращает метод объекта, который является экземпляром типа <see cref="CommandClassType"/>, для запуска.
        /// </summary>
        [CanBeNull] public readonly Func<object, Func<CommandArgs, Task>> DelegateGetter;

        /// <summary>
        /// Признак того, что команда является глобальной.
        /// Контекстная команда будет запущена только тогда, когда одна из команд в группе <see cref="CommandClassType"/> является активной в чате.
        /// Глобальная команда будет запущена только тогда, когда не имеется активных команд в чате.
        /// </summary>
        public readonly bool IsGlobal;

        private RunMetadata(Type commandType, Func<object, Func<CommandArgs, Task>> delegateGetter,
            bool isGlobal)
        {
            CommandClassType = commandType;
            DelegateGetter = delegateGetter;
            IsGlobal = isGlobal;
        }

        /// <summary>
        /// Создаёт новые метаданные о команде с помощью функции, которая возвращает асинхронный делегат с параметром.
        /// </summary>
        /// <typeparam name="T">Тип команды.</typeparam>
        /// <param name="isGlobal">Признак того, что команда является глобальной, а не контекстной.</param>
        /// <param name="delegateGetter">Функция, которая возвращает асинхронное действие для исполнения команды.</param>
        /// <returns>Новый экземпляр <see cref="RunMetadata"/>.</returns>
        public static RunMetadata CreateWithArgAndAsync<T>(bool isGlobal, Func<T, Func<CommandArgs, Task>> delegateGetter)
        {
            if (delegateGetter == null) throw new ArgumentNullException(nameof(delegateGetter));

            Func<CommandArgs, Task> FromObjectToType(object obj) => delegateGetter((T) obj);
            var type = typeof(T);

            return new RunMetadata(type, FromObjectToType, isGlobal);
        }

        /// <summary>
        /// Создаёт новые метаданные о команде с помощью функции, которая возвращает делегат с параметром.
        /// </summary>
        /// <typeparam name="T">Тип команды.</typeparam>
        /// <param name="isGlobal">Признак того, что команда является глобальной, а не контекстной.</param>
        /// <param name="delegateGetter">Функция, которая возвращает действие для исполнения команды.</param>
        /// <returns>Новый экземпляр <see cref="RunMetadata"/>.</returns>
        public static RunMetadata CreateWithArg<T>(bool isGlobal, Func<T, Action<CommandArgs>> delegateGetter)
        {
            return CreateWithArgAndAsync<T>(isGlobal, (type) =>
            {
                Task Action(CommandArgs args)
                {
                    var inner = delegateGetter(type);
                    inner(args);
                    return Task.CompletedTask;
                }

                return Action;
            });
        }

        /// <summary>
        /// Создаёт новые метаданные о команде с помощью функции, которая возвращает асинхронный делегат.
        /// </summary>
        /// <typeparam name="T">Тип команды.</typeparam>
        /// <param name="isGlobal">Признак того, что команда является глобальной, а не контекстной.</param>
        /// <param name="delegateGetter">Функция, которая возвращает асинхронное действие для исполнения команды.</param>
        /// <returns>Новый экземпляр <see cref="RunMetadata"/>.</returns>
        public static RunMetadata CreateWithAsync<T>(bool isGlobal, Func<T, Func<Task>> delegateGetter)
        {
            return CreateWithArgAndAsync<T>(isGlobal, (target) =>
            {
                Task Func(CommandArgs _) => delegateGetter(target)();
                return Func;
            });
        }

        /// <summary>
        /// Создаёт новые метаданные о команде с помощью функции, которая возвращает делегат.
        /// </summary>
        /// <typeparam name="T">Тип команды.</typeparam>
        /// <param name="isGlobal">Признак того, что команда является глобальной, а не контекстной.</param>
        /// <param name="actionGetter">Функция, которая возвращает действие для исполнения команды.</param>
        /// <returns>Новый экземпляр <see cref="RunMetadata"/>.</returns>
        public static RunMetadata Create<T>(bool isGlobal, Func<T, Action> actionGetter)
        {
            return CreateWithArgAndAsync<T>(isGlobal, (target) =>
            {
                Task Func(CommandArgs _)
                {
                    actionGetter(target)();
                    return Task.CompletedTask;
                }

                return Func;
            });
        }

        /// <summary>
        /// Создаёт новый экземпляр метаданных запуска без делегата запуска.
        /// </summary>
        public static RunMetadata CreateWithoutDelegate<T>()
        {
            return new RunMetadata(typeof(T), null, false);
        }

        protected bool Equals(RunMetadata other)
        {
            return CommandClassType == other.CommandClassType && DelegateGetter == other.DelegateGetter &&
                   IsGlobal == other.IsGlobal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RunMetadata) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CommandClassType.GetHashCode();
                hashCode = (hashCode * 397) ^ (DelegateGetter != null ? DelegateGetter.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsGlobal.GetHashCode();
                return hashCode;
            }
        }
    }
}