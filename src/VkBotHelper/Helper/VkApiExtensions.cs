using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkBotHelper.Helper
{
    public static class VkApiExtensions
    {
        /// <summary>
        /// Посылает указанное сообщение <paramref name="message"/> по указанному идентификатору чата в <paramref name="source"/>. 
        /// </summary>
        public static long SendToChat([NotNull] this Message source, [NotNull] IVkApi api, [NotNull] string message)
        {
            var msg = new MessagesSendParams() {Message = message, PeerId = source.PeerId, RandomId = new Random().Next()};
            return api.Messages.Send(msg);
        }

        /// <summary>
        /// Посылает указанное сообщение <paramref name="message"/> по указанному идентификатору чата в <paramref name="source"/>. 
        /// </summary>
        public static async Task<long> SendToChatAsync([NotNull] Message source, [NotNull] IVkApi api, [NotNull] string message)
        {
            var msg = new MessagesSendParams() {Message = message, PeerId = source.PeerId, RandomId = new Random().Next()};
            return await api.Messages.SendAsync(msg);
        }
    }
}