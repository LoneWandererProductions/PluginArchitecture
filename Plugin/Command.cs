using System.Collections.Generic;

namespace Plugin
{
    /// <summary>
    ///     Describes the command, the role and possible Input values
    /// </summary>
    public sealed class Command
    {
        public List<int> Input { get; init; }

        public string Description { get; init; }

        public bool Return { get; init; }
    }
}