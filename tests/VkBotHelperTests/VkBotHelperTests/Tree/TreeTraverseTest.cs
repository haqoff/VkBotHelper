using System.Diagnostics;
using VkBotHelper.Command.Tree;
using VkBotHelper.Parser.Tokens;
using VkBotHelper.Parser.Tokens.Values;
using VkBotHelper.Parser.Tokens.Values.Containers;
using VkBotHelperTests.TestDouble;
using Xunit;

namespace VkBotHelperTests.Tree
{
    /// <summary>
    /// Класс для тестирования алгоритма обхода дерева с получением значений.
    /// </summary>
    public class TreeTraverseTest
    {
        /// <summary>
        /// ((asd)+ asd2)+
        /// </summary>
        [Fact]
        public void TestRepeatsInsideMainRepeat()
        {
            // ((asd)+ asd2)+
            // asd asd asd2

            // asd -> asd2
            // asd -> asd
            // asd2 -> endText
            // asd2 -> asd
            var lexer = new LexerMock(
                new TokenWithValue<Date>(new Date(0, 0, 0), TokenType.Date),
                new TokenWithValue<Date>(new Date(0, 0, 0), TokenType.Date),
                new TokenWithValue<VkAtSign>(new VkAtSign(0, "a", false), TokenType.AtSign),
                new Token(TokenType.EndOfText));

            var treeStart = new CommandTreeNode(new Token(TokenType.Unknown));
            var asdNode = new CommandTreeNode(new TokenWithValue<string>("asd", TokenType.DatePlaceholder))
                {InGroupStartCount = 2, IsGroupEnd = true};
            var asd2Node = new CommandTreeNode(new TokenWithValue<string>("asd2", TokenType.AtSignPlaceholder))
                {IsGroupEnd = true};
            var endText = new CommandTreeNode(new Token(TokenType.EndOfText));

            treeStart.NextNodes.Add(asdNode.TokenPrototype, new CommandTreeNode.Reference(asdNode, false));
            asdNode.NextNodes.Add(asdNode.TokenPrototype, new CommandTreeNode.Reference(asdNode, true));
            asdNode.NextNodes.Add(asd2Node.TokenPrototype, new CommandTreeNode.Reference(asd2Node, false));

            asd2Node.NextNodes.Add(endText.TokenPrototype, new CommandTreeNode.Reference(endText, false));
            asd2Node.NextNodes.Add(asdNode.TokenPrototype, new CommandTreeNode.Reference(asdNode, true));


            var r = TreeHelper.Traverse(lexer, treeStart);

            Assert.NotNull(r.container);
            Assert.Equal(endText, r.endNode);


            /*
             * { //всё
             *      0: // 0-ой элемент - группа ((asd)+ asd2)+  <- group0
             *      {
             *          0: { //первое повторение этой группы    <- group0Iteration0
             *                  0: { // группа (asd)+           <- group1
             *                          0: {                    <- group1Iteration1
             *                                  0: asd
             *                             }           
             *                          1: {                    <- group1Iteration2
             *                                  0: asd      
             *                             }
             *                     }
             *                  1: asd2
             *             }
             *      }
             * }
             */
            Debug.Print(r.container.Count.ToString());

            Assert.Equal(1, r.container.Count);

            //группа ((asd)+ asd2)+
            var group0 = r.container.Get<GroupContainer>(0);
            // в ней 1 повторение
            Assert.Single(group0);

            //для каждого повторения в этой группе
            foreach (var group0Iteration in group0)
            {
                // внутри должна быть группа (asd)+ и токен asd2
                Assert.Equal(2, group0Iteration.Count);

                //группа (asd)+
                var group1 = group0Iteration.Get<GroupContainer>(0);
                // и в ней 2 повторения
                Assert.Equal(2, group1.Count);

                foreach (var group1Iteration in group1)
                {
                    // в этих 2 повторения по 1 элементу
                    Assert.Equal(1, group1Iteration.Count);
                }
            }
        }
    }
}