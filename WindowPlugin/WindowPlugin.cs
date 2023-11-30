using System;
using System.Collections.Generic;
using Plugin;

namespace WindowPlugin
{
    public class WindowPlugin : IPlugin
    {
        public string Name { get; } = nameof(WindowPlugin);

        public string Type { get; } = "Window";

        public Version Version { get; } = GetVersion();

        public List<Command> Commands => GetCommands();

        public int Execute()
        {
            var show = new Show();
            show.Show();
            return 0;
        }

        public object ExecuteCommand(int id)
        {
            if (Commands == null || id > Commands.Count) return null;

            return 0;
        }

        public int GetPluginType(int id)
        {
            // not yet in use so zero
            return 0;
        }

        public int Close()
        {
            return 0;
        }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        /// <returns>The Current Version</returns>
        private static Version GetVersion()
        {
            var assembly = typeof(WindowPlugin).Assembly;
            var assemblyName = assembly.GetName();

            return assemblyName.Version;
        }

        /// <summary>
        ///     Gets the commands.
        /// </summary>
        /// <returns>List of Commands</returns>
        private List<Command> GetCommands()
        {
            return null;
        }
    }
}