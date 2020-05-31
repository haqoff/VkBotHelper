using System.Collections.Generic;

namespace VkBotHelper.Command
{
    /// <summary>
    /// Представляет собой класс для сравнения метаданных запуска по их группе <see cref="RunMetadata.CommandClassType"/>.
    /// </summary>
    public class RunMetadataByGroupComparer : IEqualityComparer<RunMetadata>
    {
        public bool Equals(RunMetadata x, RunMetadata y)
        {
            return x != null && y != null && x.CommandClassType == y.CommandClassType;
        }

        public int GetHashCode(RunMetadata obj)
        {
            return obj.CommandClassType.GetHashCode();
        }
    }
}