using System.Collections.Generic;

namespace Plugin
{
    public class Command
    {
        public int Id { get; set; }

        public List<int> Input { get; set; }

        public int OutputId { get; set; }

        public int TimeOut { get; set; }

        public string Description { get; set; }

        public bool Return { get; set; }
    }
}