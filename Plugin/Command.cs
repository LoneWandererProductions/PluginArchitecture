using System.Collections.Generic;

namespace Plugin
{
    public class Command
    {
        public List<int> Input { get; set; }

        public string Description { get; set; }

        public bool Return { get; set; }
    }
}