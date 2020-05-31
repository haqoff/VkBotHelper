using System.Diagnostics;

namespace VkBotHelper.Parser
{
    /// <summary>
    /// Представляет собой класс для последовательного чтения текстовой строки.
    /// </summary>
    public class TextReader
    {
        /// <summary>
        /// Символ, выражающий что чтение очередного символа вышло за границы текста.
        /// </summary>
        public const char NullCharacter = char.MinValue;

        private readonly string _text;
        private readonly int _textLength;

        private int _currentUnreadIndex;
        private int _markedIndex;


        public TextReader(string text)
        {
            Debug.Assert(text != null);

            _text = text;
            _textLength = text.Length;
            _currentUnreadIndex = 0;
        }

        /// <summary>
        /// Получает текущий символ без продвижения вперёд.
        /// </summary>
        public char Peek()
        {
            return _currentUnreadIndex < _textLength ? _text[_currentUnreadIndex] : NullCharacter;
        }

        /// <summary>
        /// Получает символ с указанным сдвигом, относительно текущей позиции указателя, без продвижения вперёд.
        /// </summary>
        public char Peek(int offset)
        {
            var pos = _currentUnreadIndex + offset;
            return pos < _textLength ? _text[pos] : NullCharacter;
        }

        /// <summary>
        /// Считывает текущий символ и продвигает указатель вперёд.
        /// </summary>
        public char Pop()
        {
            var c = _currentUnreadIndex < _textLength ? _text[_currentUnreadIndex] : NullCharacter;
            _currentUnreadIndex++;
            return c;
        }

        /// <summary>
        /// Продвигает текущую позицию (указатель) вперёд к следующему символу.
        /// </summary>
        public void Advance()
        {
            _currentUnreadIndex++;
        }

        /// <summary>
        /// Продвигает текущую позицию (указатель) вперёд на указанное количество символов.
        /// </summary>
        public void Advance(int count)
        {
            _currentUnreadIndex += count;
        }

        /// <summary>
        /// Извлекает подстроку, начиная с текущей позиции указателя указанной длины.
        /// <remarks>
        /// Выход за диапазон длины исходной строки выбросит ошибку.
        /// </remarks>
        /// </summary>
        public string Substring(int length)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(_textLength - _currentUnreadIndex >= length);

            return _text.Substring(_currentUnreadIndex, length);
        }

        /// <summary>
        /// Извлекает подстроку, начиная с помеченной позиции и заканчивая текущей позицией указателя (не включая).
        /// </summary>
        /// <remarks>
        /// Выход за диапазон длины исходной строки выбросит ошибку.
        /// </remarks>
        public string MarkedSubstring()
        {
            Debug.Assert(_markedIndex >= 0);
            Debug.Assert(_markedIndex < _textLength);
            Debug.Assert(_textLength - _markedIndex >= _currentUnreadIndex - _markedIndex);

            return _text[_markedIndex.._currentUnreadIndex];
        }

        /// <summary>
        /// Помечает (запоминает) текущую позицию указателя.
        /// </summary>
        public void Mark() => _markedIndex = _currentUnreadIndex;

        /// <summary>
        /// Возвращает текущий указатель на помеченную позицию.
        /// </summary>
        public void ReturnToMarked() => _currentUnreadIndex = _markedIndex;

        /// <summary>
        /// Проверяет, содержит ли текст, начиная с текущей позиции указанную строку в нижнем регистре.
        /// </summary>
        /// <param name="lowerCaseString">Строка для проверки в нижнем регистре.</param>
        /// <param name="advanceIfMatch">Признак того, что необходимо продвинуть указатель, если совпадение есть.</param>
        public bool IsNextInLower(string lowerCaseString, bool advanceIfMatch = false)
        {
            Debug.Assert(lowerCaseString.ToLower() == lowerCaseString);

            if (_currentUnreadIndex + lowerCaseString.Length > _textLength) return false;

            for (var i = 0; i < lowerCaseString.Length; i++)
            {
                if (char.ToLower(_text[_currentUnreadIndex + i]) != lowerCaseString[i])
                    return false;
            }

            if (advanceIfMatch) Advance(lowerCaseString.Length);

            return true;
        }
    }
}