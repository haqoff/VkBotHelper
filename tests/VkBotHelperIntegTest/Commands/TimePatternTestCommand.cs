using System;
using VkBotHelper.Command;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VkBotHelperIntegTest.Commands
{
    internal class TimePatternTestCommand
    {
        private readonly IVkApi _api;

        public TimePatternTestCommand(IVkApi api)
        {
            _api = api;
        }

        [Command(".время #время", true)]
        public void TimeBack(CommandArgs args)
        {
            var time = args.ValueContainer.Get<TimeSpan>(0);

            _api.Messages.Send(new MessagesSendParams()
            {
                Message = time.ToString("g"),
                PeerId = args.SourceMessage.PeerId,
                RandomId = new Random().Next()
            });
        }
    }
}
