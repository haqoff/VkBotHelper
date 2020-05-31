using System;

namespace VkBotHelper.Parser.Tokens.Values.Containers
{
    /// <summary>
    /// Представляет собой базовый класс контейнер для хранения токенов со значениями.
    /// </summary>
    /// <typeparam name="T">Тип значения токена.</typeparam>
    public class GenericTokenWithValueContainer<T>
    {
        private object[] _items;

        /// <summary>
        /// Количество элементов в контейнере.
        /// </summary>
        public int Count => _items.Length;

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="GenericTokenWithValueContainer{T}"/> с указанными элементами.
        /// </summary>
        /// <param name="items">Массив элементов, которые должны быть типом <see cref="TokenWithValue{T}"/>.</param>
        internal GenericTokenWithValueContainer(object[] items)
        {
            _items = items;
        }

        /// <summary>
        /// Получает значение по указанному индексу.
        /// </summary>
        /// <typeparam name="TG">Тип получаемого значения.</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">Исключение, которые выдаётся, если индекс находится за пределами допустимых значений.</exception>
        /// <exception cref="InvalidCastException">Исключение, которое выдаётся в случае если ожидаемый тип не равен актуальному.</exception>
        /// <returns></returns>
        public TG Get<TG>(int index) where TG : T
        {
            return GetToken<TG>(index).Value;
        }

        /// <summary>
        /// Получает токен по указанному индексу.
        /// </summary>
        /// <typeparam name="TG">Тип значения токена.</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">Исключение, которые выдаётся, если индекс находится за пределами допустимых значений.</exception>
        /// <exception cref="InvalidCastException">Исключение, которое выдаётся в случае если ожидаемый тип не равен актуальному.</exception>
        /// <returns>Токен.</returns>
        public TokenWithValue<TG> GetToken<TG>(int index) where TG : T
        {
            if (index < 0 || index >= _items.Length)
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"Переданный индекс '{index}' находится за границами. Всего элементов: {Count}.");

            var item = _items[index];
            if (item is TokenWithValue<TG> castItem)
            {
                return castItem;
            }

            throw new InvalidCastException(
                $"Элемент по индексу '{index}' не является типом {typeof(TG).Name}. Актуальный тип этого элемента - {item.GetType().Name}.");
        }

        /// <summary>
        /// Добавляет указанный объект в контейнер.
        /// </summary>
        /// <param name="token">Токен со значением - <see cref="TokenWithValue{T}"/>.</param>
        internal void AddToken(object token)
        {
            Array.Resize(ref _items, _items.Length + 1);
            _items[^1] = token;
        }
    }
}