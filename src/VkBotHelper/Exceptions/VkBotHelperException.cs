using System;

namespace VkBotHelper.Exceptions
{
    /// <summary>
    /// Представляет собой базовую ошибку, выбрасываемую библиотекой.
    /// </summary>
    public class VkBotHelperException : Exception
    {
        public VkBotHelperException(string msg) : base(msg)
        {
        }
    }
}