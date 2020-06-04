using System.Linq;
using System.Threading.Tasks;
using VkBotHelper.Command;
using VkBotHelper.Command.Tree;
using VkBotHelper.Menu;
using Xunit;

namespace VkBotHelperUnitTests.Tree
{
    public class CommandTreeBuilderTest
    {
        private class CommandFake
        {
            public bool ESyncWithArg = false;
            public bool EAsyncWithArg = false;
            public bool ESyncWithoutArg = false;
            public bool EAsyncWithoutArg = false;
            public bool EMenuItem = false;

            [Command("1", true)]
            public void SyncWithArg(CommandArgs args)
            {
                ESyncWithArg = true;
            }

            [Command("2", true)]
            public Task AsyncWithArg(CommandArgs args)
            {
                EAsyncWithArg = true;
                return Task.CompletedTask;
            }

            [Command("3", true)]
            public void SyncWithoutArg()
            {
                ESyncWithoutArg = true;
            }

            [Command("4", false)]
            public Task AsyncWithoutArg()
            {
                EAsyncWithoutArg = true;
                return Task.CompletedTask;
            }

            [CommandMenuItem("5", "text")]
            public void TestCommandMenuItemAttribute()
            {
                EMenuItem = true;
            }
        }

        [Fact]
        public async Task TestExtractCommandsByAttribute()
        {
            var commands = CommandTreeBuilder.ExtractCommandsFromClass<CommandFake>().ToArray();
            var patterns = commands.Select(c => c.pattern).ToArray();

            Assert.Contains("1", patterns);
            Assert.Contains("2", patterns);
            Assert.Contains("3", patterns);
            Assert.Contains("4", patterns);
            Assert.Contains("5", patterns);

            var testInstance = new CommandFake();
            foreach (var (pattern, metadata) in commands)
            {
                Assert.NotNull(metadata.DelegateGetter);

                var action = metadata.DelegateGetter(testInstance);
                
                Assert.NotNull(action);

                await action(null);
            }

            Assert.True(testInstance.EAsyncWithArg);
            Assert.True(testInstance.EAsyncWithoutArg);
            Assert.True(testInstance.ESyncWithArg);
            Assert.True(testInstance.ESyncWithoutArg);
            Assert.True(testInstance.EMenuItem);
        }
    }
}
