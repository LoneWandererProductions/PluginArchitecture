using System;
using System.Collections.Generic;
using Plugin;

namespace WindowPlugin
{
    public class WindowPlugin : IPlugin
    {
        private Show _show;

        public string Name { get; } = nameof(WindowPlugin);

        public string Type { get; } = "Window";

        public string Description { get; } = "Displays a Window";

        public Version Version { get; } = GetVersion();

        public List<Command> Commands => GetCommands();

        public int Execute()
        {
            _show = new Show();

            return 0;
        }

        public object ExecuteCommand(int id)
        {
            if (id != 0) return null;

            _show.Show();

            return 0;
        }

        public int GetPluginType(int id)
        {
            // not yet in use so zero
            return 0;
        }

        public string GetInfo()
        {
            return string.Concat(Type, Environment.NewLine, Version, Environment.NewLine, Description);
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
            var comOne = new Command
            {
                Description = "Start a window",
                Return = false
            };

            return new List<Command> {comOne};
        }
    }
}