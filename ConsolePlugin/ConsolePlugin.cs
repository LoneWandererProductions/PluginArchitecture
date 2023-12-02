/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugin
 * FILE:        ConsolePlugin/ConsolePlugin.cs
 * PURPOSE:     Test Plugin
 * PROGRAMER:   Peter Geinitz (Wayfarer)
 */

using System;
using System.Collections.Generic;
using Plugin;

namespace ConsolePlugin
{
    public class ConsolePlugin : IPlugin
    {
        public string Name { get; } = nameof(ConsolePlugin);

        public string Description { get; } = "Does some work in the Background.";

        public Version Version { get; } = GetVersion();

        public List<Command> Commands => GetCommands();

        public string Type { get; } = "Console";

        public int Execute()
        {
            return 0;
        }

        public object ExecuteCommand(int id)
        {
            if (Commands == null || id > Commands.Count) return -1;

            var com = Commands[id];
            object result;

            switch (id)
            {
                case 0:
                    result = PluginHelper.DoMagicOne(com);
                    break;

                case 2:
                    result = PluginHelper.DoMagicTwo(com);
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
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
            var comOne = new Command
            {
                Description = "Some string manipulations",
                Input = new List<int> {0},
                Return = true
            };

            var comTwo = new Command
            {
                Description = "Get other input values",
                Input = new List<int> {1, 2},
                Return = true
            };

            return new List<Command> {comOne, comTwo};
        }
    }
}