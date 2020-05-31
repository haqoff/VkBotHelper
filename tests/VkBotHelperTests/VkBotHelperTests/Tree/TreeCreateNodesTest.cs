using VkBotHelper.Command.Tree;
using VkBotHelper.Parser.Tokens;
using VkBotHelperTests.TestDouble;
using Xunit;

namespace VkBotHelperTests.Tree
{
    public class TreeCreateNodesTest
    {
        [Fact]
        public void TestSourcePatternDifferent1()
        {
            /**
             * pattern: #дата, 
             */
        }

        /// <summary>
        /// ((n1)+ n2)+
        /// </summary>
        [Fact]
        public void TestRepeatInsideRepeat()
        {
            /*
             * pattern:
             *  ((n1)+ n2)+
             * expect tree:
             *  (n1) -> (n1)
             *  (n1) -> (n2)
             *  (n2) -> (n1)
             *  (n2) -> (EOT)
             */

            var n1 = new TokenWithValue<string>("n1", TokenType.CommandText);
            var n2 = new TokenWithValue<string>("n2", TokenType.CommandText);

            var tokens =
                new[]
                {
                    new Token(TokenType.OpenParen),
                    new Token(TokenType.OpenParen),
                    n1,
                    new Token(TokenType.CloseParen),
                    new Token(TokenType.Plus),
                    n2,
                    new Token(TokenType.CloseParen),
                    new Token(TokenType.Plus),
                    new Token(TokenType.EndOfText)
                };


            var startNode = new CommandTreeNode(new Token(TokenType.Unknown));
            var endNode = TreeHelper.CreateTreeNodes(new LexerMock(tokens), startNode);
            Assert.Equal(TokenType.EndOfText, endNode.TokenPrototype.Type);

            if (startNode.NextNodes.TryGetValue(n1, out var n1Reference))
            {
                var n1Node = n1Reference.Node;
                if (n1Node.NextNodes.TryGetValue(n2, out var n2Reference))
                {
                    var n2Node = n2Reference.Node;
                    if (!n2Node.NextNodes.ContainsKey(n1))
                    {
                        Assert.True(false, "n2 -> n1");
                    }

                    if (!n2Node.NextNodes.ContainsKey(new Token(TokenType.EndOfText)))
                    {
                        Assert.True(false, "n2 -> eof");
                    }
                }
                else
                {
                    Assert.True(false, "n1 -> n2");
                }

                if (!n1Node.NextNodes.ContainsKey(n1))
                {
                    Assert.True(false, "n1 -> n1");
                }
            }
            else
            {
                Assert.True(false, "start -> n1");
            }
        }
    }
}