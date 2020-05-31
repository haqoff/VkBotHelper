using VkBotHelper.Command.Tree;
using VkBotHelper.Parser.Tokens;

namespace VkBotHelperTests
{
    public static class Utils
    {
        internal static CommandTreeNode NodeWithChildren(CommandTreeNode node, params CommandTreeNode[] children)
        {
            foreach (var child in children)
            {
                node.NextNodes.Add(child.TokenPrototype, new CommandTreeNode.Reference(child, false));
            }

            return node;
        }

        internal static Token Token(TokenType t) => new Token(t);
    }
}