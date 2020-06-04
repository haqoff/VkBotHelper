using Microsoft.Extensions.Configuration;
using VkBotHelper.Helper;
using VkBotHelperIntegTest.Commands;

namespace VkBotHelperIntegTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("test-settings.json", false, false)
                .Build();

            var groupAccessToken = config["accessToken"];
            var botGroupId = ulong.Parse(config["botGroupId"]);
            var dbConnectionString = config["dbConnection"];

            VkBot.StartNewCommandBot(groupAccessToken, botGroupId, builder =>
                {
                    builder.Register<HelloCommand>();
                    builder.Register<TimePatternTestCommand>();
                },
                container => { });
        }
    }
}