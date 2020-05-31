using System.Collections.Generic;

namespace VkBotHelper.Collections
{
    /// <summary>
    /// Представляет собой словарь с возможностью хранения нескольких неодинаковых значений на ключ.
    /// </summary>
    /// <typeparam name="TK">Тип ключа.</typeparam>
    /// <typeparam name="TV">Тип значения.</typeparam>
    public class MultiValueDictionary<TK, TV>
    {
        private readonly Dictionary<TK, HashSet<TV>> _dictionary;
        private readonly IEqualityComparer<TV> _valueComparer;

        public MultiValueDictionary() : this(EqualityComparer<TV>.Default)
        {
        }

        public MultiValueDictionary(IEqualityComparer<TV> valueComparer)
        {
            _dictionary = new Dictionary<TK, HashSet<TV>>();
            _valueComparer = valueComparer;
        }

        /// <summary>
        /// Определяет, содержит ли словарь указанный ключ.
        /// </summary>
        public bool ContainsKey(TK key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Определяет, содержит ли словарь указанные ключ и значение.
        /// </summary>
        public bool ContainsKeyValue(TK key, TV value)
        {
            return _dictionary.TryGetValue(key, out var set) && set.Contains(value);
        }

        /// <summary>
        /// Добавляет указанные ключ и значение в словарь.
        /// </summary>
        public void Add(TK key, TV value)
        {
            if (!_dictionary.TryGetValue(key, out var set))
            {
                set = new HashSet<TV>(_valueComparer);
                _dictionary.Add(key, set);
            }

            set.Add(value);
        }

        /// <summary>
        /// Добавляет указанные ключ и значения для этого ключа в словарь.
        /// </summary>
        public void Add(TK key, IEnumerable<TV> values)
        {
            if (!_dictionary.TryGetValue(key, out var set))
            {
                set = new HashSet<TV>(_valueComparer);
                _dictionary.Add(key, set);
            }

            foreach (var value in values)
            {
                set.Add(value);
            }
        }

        /// <summary>
        /// Удаляет указанный ключ вместе со всеми значениями из словаря.
        /// </summary>
        /// <returns><see langword="true"/>, если такой ключ содержался в словаре. Иначе - <see langword="false"/>.</returns>
        public bool RemoveKey(TK key)
        {
            return _dictionary.Remove(key);
        }

        /// <summary>
        /// Удаляет указанное значение по ключу из словаря.
        /// </summary>
        /// <returns><see langword="true"/>, если такие ключ и значение содержались в словаре. Иначе - <see langword="false"/>.</returns>
        public bool RemoveKeyValue(TK key, TV value)
        {
            if (!_dictionary.TryGetValue(key, out var set)) return false;
            if (!set.Remove(value)) return false;

            if (set.Count == 0)
            {
                _dictionary.Remove(key);
            }

            return true;
        }

        public bool TryGetActualValue(TK key, TV equalValue, out TV actualValue)
        {
            actualValue = default;
            return _dictionary.TryGetValue(key, out var set) && set.TryGetValue(equalValue, out actualValue);
        }

        /// <summary>
        /// Получает коллекцию значений по указанному ключу.
        /// </summary>
        public bool TryGetValues(TK key, out IReadOnlyCollection<TV> values)
        {
            if (_dictionary.TryGetValue(key, out var set))
            {
                values = set;
                return true;
            }

            values = null;
            return false;
        }
    }
}