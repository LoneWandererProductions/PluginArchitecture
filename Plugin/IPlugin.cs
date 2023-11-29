/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugin
 * FILE:        Plugin/IPlugin.cs
 * PURPOSE:     Basic Plugin Support
 * PROGRAMER:   Peter Geinitz (Wayfarer)
 * SOURCES:     https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support
 */

using System;
using System.Collections.Generic;

namespace Plugin
{
    /// <summary>
    ///     Plugin Interface Implementation
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        string Name { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        string Type { get; }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        /// <value>
        ///     The version.
        /// </value>
        Version Version { get; }

        /// <summary>
        ///     Gets the possible commands for the Plugin.
        /// </summary>
        /// <value>
        ///     The commands that the main module can call from the plugin.
        /// </value>
        List<Command> Commands { get; }

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        /// <returns>Status Code</returns>
        int Execute();

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <param name="id">The identifier of the command.</param>
        /// <returns>Status Code</returns>
        object ExecuteCommand(int id);

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        /// <returns>Status Code</returns>
        int Close();
    }
}