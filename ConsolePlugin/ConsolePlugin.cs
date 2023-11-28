using System;
using System.Collections.Generic;
using Plugin;

namespace ConsolePlugin
{
    public class ConsolePlugin : IPlugin
    {
        internal static Dictionary<int, CommandResult> Result { get; set; }

        public string Name { get; } = nameof(ConsolePlugin);

        public Version Version { get; } = GetVersion();

        public List<Command> Commands => GetCommands();

        public string Type { get; } = "Console";

        public int Execute()
        {
            return 0;
        }

        public int ExecuteCommand(int id)
        {
            if (Commands == null || id > Commands.Count) return -1;


            switch (id)
            {
                case 0:
                    PluginHelper.DoMagic();
                    break;
            }

            return 0;
        }

        public int Close()
        {
            return 0;
        }

        public CommandResult GetValue(int id)
        {
            return !Result.ContainsKey(id) ? null : Result[id];
        }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        /// <returns>The Current Version</returns>
        private static Version GetVersion()
        {
            var assembly = typeof(ConsolePlugin).Assembly;
            var assemblyName = assembly.GetName();

            return assemblyName.Version;
        }

        /// <summary>
        ///     Gets the commands.
        /// </summary>
        /// <returns>List of Commands</returns>
        private static List<Command> GetCommands()
        {
            var lst = new List<Command>();
            var com = new Command
            {
                Description = "Some string manipulations",
                //com.Id = 0; should be set at runtime by the master module
                //com.OutputId = 0; should be set at runtime by the master module
                Input = new List<int> {0},
                Return = true,
                TimeOut = 1000
            };


            lst.Add(com);
            return lst;
        }
    }
}