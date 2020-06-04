using System;
using VkBotHelper.Command;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VkBotHelperIntegTest.Commands
{
    internal class HelloCommand
    {
        private readonly IVkApi _api;

        public HelloCommand(IVkApi api)
        {
            _api = api;
        }

        [Command(".привет", true)]
        public void Hi(CommandArgs args)
        {
            _api.Messages.Send(new MessagesSendParams()
            {
                Message = "привет-привет",
                PeerId = args.SourceMessage.PeerId,
                RandomId = new Random().Next()
            });
        }
    }
}