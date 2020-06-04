using System;
using System.Diagnostics;
using System.Text;
using VkBotHelper.Parser.Tokens;
using VkBotHelper.Parser.Tokens.Values;

namespace VkBotHelper.Parser
{
    /// <summary>
    /// Представляет собой класс для лексического анализа текста.
    /// </summary>
    public class Lexer : ISourceLexer, IPatternLexer
    {
        private readonly TextReader _reader;
        private readonly StringBuilder _sb;
        private Token _curScanned;

        public Lexer(TextReader reader)
        {
            Debug.Assert(reader != null);

            _reader = reader;
            _sb = new StringBuilder();
        }


        #region Source

        /// <summary>
        /// Получает следующий токен, разбирая исходный текст.
        /// </summary>
        public Token NextSourceToken()
        {
            SkipWhiteSpace();
            ScanSource();
            return _curScanned;
        }

        /// <summary>
        /// Сканирует исходный текст на новую лексему.
        /// </summary>
        private void ScanSource()
        {
            var c = _reader.Peek();

            switch (c)
            {
                case '.':
                    _curScanned = new Token(TokenType.Dot);
                    _reader.Advance();
                    break;
                case '/':
                    _curScanned = new Token(TokenType.Slash);
                    _reader.Advance();
                    break;

                case '!':
                    _curScanned = new Token(TokenType.Exclamation);
                    _reader.Advance();
                    break;

                case TextReader.NullCharacter:
                    _curScanned = new Token(TokenType.EndOfText);
                    break;

                // #строка
                case '\'':
                case '"':
                case '«':
                    ScanSourceStringLiteral();
                    break;

                /**
                 * Только если после символов '-' и '+' обязательно идёт цифра, можем сканировать их как числовой вид.
                 */
                case '-' when char.IsDigit(_reader.Peek(1)):
                case '+' when char.IsDigit(_reader.Peek(1)):
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ScanSourceNumeric();
                    break;

                // #обращение
                case '[':
                    if (!ScanSourceAtSign()) ScanSourceText();
                    break;

                default:
                    // если не получилось разобрать как #день_недели или #день_смещение, сканируем как обычный текст
                    if (!ScanDayOfWeekOffset() && !ScanFromTodayOffset())
                    {
                        ScanSourceText();
                    }

                    break;
            }
        }

        /// <summary>
        /// Сканирует строковой литерал в исходном тексте.
        /// </summary>
        private void ScanSourceStringLiteral()
        {
            const char nullCharacter = TextReader.NullCharacter; //храним локально в стеке для повышения производительности

            char openTag = _reader.Pop();
            var closeTag = openTag switch
            {
                '«' => '»',
                _ => openTag
            };

            var stringLength = 0;

            while (true)
            {
                var c = _reader.Peek(stringLength);

                if (c == closeTag)
                {
                    break;
                }

                if (c == nullCharacter)
                {
                    // TODO: добавить ошибку в токен о потери закрывающей кавычки.
                    break;
                }

                stringLength++;
            }

            var literal = _reader.Substring(stringLength);
            _reader.Advance(stringLength + 1);
            _curScanned = new TokenWithValue<string>(literal, TokenType.StringLiteral);
        }

        /// <summary>
        /// Сканирует числовую последовательность в исходном тексте - все лексемы, которые начинаются с цифры.
        /// </summary>
        /// <remarks>
        /// #дата #время #дробное_число #целое_число
        /// </remarks>
        private void ScanSourceNumeric()
        {
            var length = 0;

            var hasDateTag = false;
            var hasTimeTag = false;
            var hasPrefix = false;

            var done = false;
            while (!done)
            {
                var c = _reader.Peek(length);
                switch (c)
                {
                    // вызов char.IsDigit является затратной операцией
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        length++;
                        break;
                    /**
                     * Можем считывать эти символы, только когда они являются первыми
                     * и после них обязательно идёт цифра - в ScanSource до этого уже была такая проверка.
                     */
                    case '-' when length == 0:
                    case '+' when length == 0:
                        Debug.Assert(char.IsDigit(_reader.Peek(1)));
                        hasPrefix = true;
                        length++;
                        break;

                    /**
                     * Формат #дата поддерживает следующие виды разделяющих символов: 01/01/2001 или 01-01-2001 или 01.01.2001.
                     * Символ '-' при этом относим к возможным символам даты только если он не является первым
                     */
                    case '/':
                    case '-' when length != 0:
                    case '.':
                        hasDateTag = true;
                        length++;
                        break;

                    /**
                     * #время принимает следующий вид через символ ':' - 12:30
                     */
                    case ':':
                        Debug.Assert(length != 0); // символ ':' не может быть первым, но этого не должен допускать ScanSource 
                        length++;
                        hasTimeTag = true;
                        break;

                    /*
                     * #число может быть дробным и задаваться через запятую,
                     * которую учитываем, если за ней цифра
                     */
                    case ',' when char.IsDigit(_reader.Peek(length + 1)):
                        length++;
                        break;

                    default:
                        done = true;
                        break;
                }
            }

            var scannedText = _reader.Substring(length);
            _reader.Advance(length);

            if (hasDateTag && !hasPrefix)
            {
                if (DateTime.TryParse(scannedText, out DateTime dateValue))
                {
                    _curScanned = new TokenWithValue<Date>(new Date(dateValue.Day, dateValue.Month, dateValue.Year),
                        TokenType.Date);
                }
                else
                {
                    // TODO: добавить в токен информацию об ошибке разбора дат.
                    _curScanned = new Token(TokenType.Unknown);
                }
            }
            else if (hasTimeTag && !hasPrefix)
            {
                if (TimeSpan.TryParse(scannedText, out var timeValue))
                {
                    _curScanned = new TokenWithValue<TimeSpan>(timeValue, TokenType.Time);
                }
                else
                {
                    // TODO: добавить в токен информацию об ошибке разбора времени.
                    _curScanned = new Token(TokenType.Unknown);
                }
            }
            else
            {
                if (double.TryParse(scannedText, out var doubleValue))
                {
                    _curScanned = new TokenWithValue<double>(doubleValue, TokenType.DoubleLiteral);
                }
                else
                {
                    // TODO: добавить в токен информацию об ошибке разбора вещественного числа.
                    _curScanned = new Token(TokenType.Unknown);
                }
            }
        }

        /// <summary>
        /// Сканирует исходный текст на обращение ВКонтакте.
        /// </summary>
        /// <remarks>
        /// Обращение ВКонтакте имеет следующий текстовый вид: [club/id идентификатор|отображаемое имя].
        /// </remarks>
        /// <returns><see langword="true"/>, если удалось разобрать на токен, иначе - <see langword="false"/>.</returns>
        private bool ScanSourceAtSign()
        {
            string ScanUntil(char expectedChar, int minLength, int maxLength)
            {
                var scannedLength = 0;

                while (true)
                {
                    if (scannedLength > maxLength) return null;

                    var cur = _reader.Peek(scannedLength);
                    if (cur == TextReader.NullCharacter) return null;

                    if (cur == expectedChar) break;

                    scannedLength++;
                }

                if (scannedLength < minLength)
                    return null;

                var text = _reader.Substring(scannedLength);
                _reader.Advance(scannedLength + 1); //продвигаемся на длину отсканированной строки + expectedChar
                return text;
            }

            _reader.Mark();
            _reader.Advance(); //продвигаем '['

            bool isClub;
            if (_reader.IsNextInLower("id", true))
            {
                isClub = false;
            }
            else if (_reader.IsNextInLower("club", true))
            {
                isClub = true;
            }
            else
            {
                _reader.ReturnToMarked();
                return false;
            }

            var idString = ScanUntil('|', 1, 24);
            if (idString == null || !long.TryParse(idString, out var id))
            {
                _reader.ReturnToMarked();
                return false;
            }

            var displayName = ScanUntil(']', 1, 64);
            if (displayName == null)
            {
                _reader.ReturnToMarked();
                return false;
            }

            _curScanned = new TokenWithValue<VkAtSign>(new VkAtSign(id, displayName, isClub), TokenType.AtSign);
            return true;
        }

        /// <summary>
        /// Сканирует #день_недели - смещение относительно понедельника текущей недели.
        /// </summary>
        /// <example>
        /// Например, выражение вида "следующий вторник" образует токен со значением 8 - именно столько дней до следующего вторника, с понедельника текущей недели.
        /// Выражение вида "пред среда" примет значение -5.
        /// </example>
        /// <remarks>
        /// Поддерживает формы префиксов: пред и предыдущий, след и следующий. Дни недели при этом также поддерживают краткую форму. 
        /// </remarks>
        /// <returns><see langword="true"/>, если удалось разобрать на токен, иначе - <see langword="false"/>.</returns>
        private bool ScanDayOfWeekOffset()
        {
            var weekOffset = 0;
            var dayBase = -1;

            int ScanPrefix(string fullForm, string shortForm)
            {
                var countFound = 0;

                while (true)
                {
                    if (_reader.IsNextInLower(fullForm, true))
                    {
                        countFound++;
                        //между повторяющимися префиксами разрешён пробел (хоть и не обязателен), пропускаем их
                        SkipWhiteSpace();
                        continue;
                    }

                    if (_reader.IsNextInLower(shortForm, true))
                    {
                        countFound++;
                        SkipWhiteSpace();
                        continue;
                    }

                    break;
                }

                return countFound;
            }

            _reader.Mark();

            weekOffset += ScanPrefix("следующий", "след");
            if (weekOffset == 0) weekOffset -= ScanPrefix("предыдущий", "пред");

            if (_reader.IsNextInLower("понедельник", true) || _reader.IsNextInLower("пн", true))
            {
                dayBase = 0;
            }
            else if (_reader.IsNextInLower("вторник", true) || _reader.IsNextInLower("вт", true))
            {
                dayBase = 1;
            }
            else if (_reader.IsNextInLower("среда", true) || _reader.IsNextInLower("ср", true))
            {
                dayBase = 2;
            }
            else if (_reader.IsNextInLower("четверг", true) || _reader.IsNextInLower("чт", true))
            {
                dayBase = 3;
            }
            else if (_reader.IsNextInLower("пятница", true) || _reader.IsNextInLower("пт", true))
            {
                dayBase = 4;
            }
            else if (_reader.IsNextInLower("суббота", true) || _reader.IsNextInLower("сб", true))
            {
                dayBase = 5;
            }
            else if (_reader.IsNextInLower("воскресенье", true) || _reader.IsNextInLower("вс", true))
            {
                dayBase = 6;
            }

            if (dayBase > -1)
            {
                var totalDayOfWeekOffset = dayBase + 7 * weekOffset;
                _curScanned = new TokenWithValue<int>(totalDayOfWeekOffset, TokenType.DayOfWeekOffset);

                return true;
            }

            // если не нашли день недели, то возвращаем указатель назад, игнорируя в т.ч. все префиксы
            _reader.ReturnToMarked();
            return false;
        }

        /// <summary>
        /// Сканирует смещение, выраженное в днях, относительно текущего.
        /// </summary>
        /// <example>
        /// Выражение вида "послепослезавтра" примет токен со значением 3.
        /// Выражение вида "позавчера" примет токен со значением -2.
        /// </example>
        /// <remarks>
        /// Принимаются формы: [после]*завтра, [поза]*вчера, без пробелов.
        /// </remarks>
        /// <returns><see langword="true"/>, если удалось разобрать на токен, иначе - <see langword="false"/>.</returns>
        private bool ScanFromTodayOffset()
        {
            int ScanPrefix(string val)
            {
                var countFound = 0;
                while (_reader.IsNextInLower(val, true)) countFound++;

                return countFound;
            }

            _reader.Mark();

            var dayOffset = 0;
            dayOffset += ScanPrefix("после");
            if (dayOffset == 0) dayOffset -= ScanPrefix("поза");

            if (dayOffset <= 0 && _reader.IsNextInLower("вчера", true))
            {
                dayOffset--;
                _curScanned = new TokenWithValue<int>(dayOffset, TokenType.FromTodayOffset);

                return true;
            }

            if (dayOffset >= 0 && _reader.IsNextInLower("завтра", true))
            {
                dayOffset++;
                _curScanned = new TokenWithValue<int>(dayOffset, TokenType.FromTodayOffset);

                return true;
            }

            /**
             * если нет 'вчера' или 'завтра', возвращаемся на позицию начала сканирования, в т.ч. игнорируя все 'поза' и 'после' - не считая за ошибку
             * а даём обработать их дальше, например, как текст команды
             */
            _reader.ReturnToMarked();
            return false;
        }

        /// <summary>
        /// Сканирует исходной текст на токен текстовой последовательности.
        /// </summary>
        private void ScanSourceText()
        {
            _reader.Mark();

            const char nullCharacter = TextReader.NullCharacter;
            var done = false;

            while (!done)
            {
                var ch = _reader.Peek();
                switch (ch)
                {
                    // эти символы не могут быть в исходном тексте
                    case '!':
                    case '.':
                    case '/':
                    case nullCharacter:
                        done = true;
                        break;
                    default:
                        if (IsWhiteSpace(ch))
                        {
                            done = true;
                        }
                        else
                        {
                            _reader.Advance();
                        }

                        break;
                }
            }

            var text = _reader.MarkedSubstring();
            _curScanned = new TokenWithValue<string>(text, TokenType.CommandText);
        }

        #endregion

        #region Pattern

        /// <summary>
        /// Получает следующий токен, разбирая текст как шаблон команды.
        /// </summary>
        public Token NextPatternToken()
        {
            SkipWhiteSpace();
            ScanPattern();
            return _curScanned;
        }

        /// <summary>
        /// Сканирует текст шаблона на новую лексему.
        /// </summary>
        private void ScanPattern()
        {
            var c = _reader.Peek();
            switch (c)
            {
                case '/':
                    var next = _reader.Peek(1);
                    switch (next)
                    {
                        // если один из этих символов идёт после slash, то это экранирование, сканируем как строку
                        case '(':
                        case ')':
                        case '+':
                        case '#':
                            ScanPatternText();
                            break;
                        default:
                            _curScanned = new Token(TokenType.Slash);
                            _reader.Advance();
                            break;
                    }

                    break;
                case '(':
                    _curScanned = new Token(TokenType.OpenParen);
                    _reader.Advance();
                    break;
                case ')':
                    _curScanned = new Token(TokenType.CloseParen);
                    _reader.Advance();
                    break;
                case '+':
                    _curScanned = new Token(TokenType.Plus);
                    _reader.Advance();
                    break;
                case '#':
                    ScanPatternPlaceholder();
                    break;
                case '.':
                    _curScanned = new Token(TokenType.Dot);
                    _reader.Advance();
                    break;
                case '!':
                    _curScanned = new Token(TokenType.Exclamation);
                    _reader.Advance();
                    break;
                case TextReader.NullCharacter:
                    _curScanned = new Token(TokenType.EndOfText);
                    break;
                default:
                    ScanPatternText();
                    break;
            }
        }

        /// <summary>
        /// Сканирует текст шаблона на текстовую последовательность.
        /// </summary>
        private void ScanPatternText()
        {
            _sb.Clear();
            var scannedLength = 0;
            var done = false;

            while (!done)
            {
                var c = _reader.Peek(scannedLength);
                switch (c)
                {
                    /**
                     * эти символы не могут находится в тексте без escape-slash.
                     * если встречаем, заканчиваем сканирование текста                       
                     */
                    case '(':
                    case ')':
                    case '+':
                    case '#':
                    case TextReader.NullCharacter:
                        done = true;
                        break;
                    case '/':
                        var next = _reader.Peek(scannedLength + 1);
                        switch (next)
                        {
                            // если один из этих символов находится после slash, то это экранирование.
                            case '(':
                            case ')':
                            case '+':
                            case '#':
                                scannedLength += 2;
                                _sb.Append(next);
                                break;
                            default:
                                // если slash не экранирует, то заканчиваем сканирование.
                                done = true;
                                break;
                        }

                        break;
                    default:
                        if (IsWhiteSpace(c))
                        {
                            done = true;
                        }
                        else
                        {
                            _sb.Append(c);
                            scannedLength++;
                        }

                        break;
                }
            }

            _reader.Advance(scannedLength);
            var text = _sb.ToString();
            _curScanned = new TokenWithValue<string>(text, TokenType.CommandText);
        }

        /// <summary>
        /// Сканирует текст шаблона на токен плейсхолдера.
        /// </summary>
        private void ScanPatternPlaceholder()
        {
            var name = GetStringUntilWhiteSpace();

            _curScanned = name switch
            {
                "#дата" => new Token(TokenType.DatePlaceholder),
                "#время" => new Token(TokenType.TimePlaceholder),
                "#строка" => new Token(TokenType.StringLiteralPlaceholder),
                "#число" => new Token(TokenType.DoubleLiteralPlaceholder),
                "#обращение" => new Token(TokenType.AtSignPlaceholder),
                "#день_смещение" => new Token(TokenType.FromTodayOffsetPlaceholder),
                "#день_недели" => new Token(TokenType.DayOfWeekOffsetPlaceholder),
                _ => new Token(TokenType.Unknown) // TODO: добавить в токен ошибку на неизвестный placeholder.
            };
        }

        #endregion

        private string GetStringUntilWhiteSpace()
        {
            _reader.Mark();

            while (true)
            {
                var c = _reader.Peek();
                if (IsWhiteSpace(c) || c == TextReader.NullCharacter)
                    break;

                _reader.Advance();
            }

            return _reader.MarkedSubstring();
        }

        /// <summary>
        /// Пропускает все пробельные символы.
        /// </summary>
        private void SkipWhiteSpace()
        {
            while (IsWhiteSpace(_reader.Peek())) _reader.Advance();
        }

        /// <summary>
        /// Определяет, является ли указанный символ пробелом.
        /// </summary>
        private static bool IsWhiteSpace(char c)
        {
            switch (c)
            {
                case ' ':
                case '\n':
                case '\r':
                case '\t':
                case '\f':
                case '\u001A':
                    return true;

                default:
                    return false;
            }
        }
    }
}