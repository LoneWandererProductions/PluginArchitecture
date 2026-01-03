/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugin
 * FILE:        PluginLoader/PluginLoader.cs
 * PURPOSE:     Basic Plugin Support, Load all Plugins
 * PROGRAMER:   Peter Geinitz (Wayfarer)
 * SOURCES:     https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support
 */

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Plugins.Interfaces;

namespace PluginLoader
{
    /// <summary>
    ///     Basic Load System for the Plugins
    /// </summary>
    public static class PluginLoad
    {
        /// <summary>
        ///     The load error event
        /// </summary>
        public static EventHandler? loadErrorEvent;

        /// <summary>
        ///     Gets or sets the plugin container.
        /// </summary>
        /// <value>
        ///     The plugin container.
        /// </value>
        public static List<IPlugin> PluginContainer { get; private set; }

        /// <summary>
        ///     Loads all.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="extension">The extension for plugins.</param>
        /// <returns>
        ///     Success Status
        /// </returns>
        public static bool LoadAll(string path, string extension = PluginLoaderResources.FileExt)
        {
            var pluginPaths = GetFilesByExtensionFullPath(path, extension);

            if (pluginPaths == null)
            {
                return false;
            }

            PluginContainer = new List<IPlugin>();

            foreach (var pluginPath in pluginPaths)
            {
                var pluginAssembly = LoadPlugin(pluginPath);

                try
                {
                    // Get all types that implement IPlugin and are not abstract

                }
                catch (Exception ex) when (ex is ArgumentException or FileLoadException or ApplicationException
                                               or ReflectionTypeLoadException or BadImageFormatException
                                               or FileNotFoundException)
                {
                    Trace.WriteLine(ex);
                    loadErrorEvent?.Invoke(nameof(LoadAll), new LoaderErrorEventArgs(ex.ToString()));
                }
            }

            return PluginContainer.Count != 0;
        }

        /// <summary>
        ///     Gets the files by extension full path.
        ///     Adopted from FileHandler to decrease dependencies
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="extension">The custom file extension.</param>
        /// <returns>List of files by extension with full path</returns>
        private static IEnumerable<string> GetFilesByExtensionFullPath(string path, string extension)
        {
            if (string.IsNullOrEmpty(path))
            {
                Trace.WriteLine(PluginLoaderResources.ErrorPath);
                return null;
            }

            if (Directory.Exists(path))
            {
                return Directory.EnumerateFiles(path, $"*{extension}", SearchOption.TopDirectoryOnly).ToList();
            }

            Trace.WriteLine(PluginLoaderResources.ErrorDirectory);

            return null;
        }

        /// <summary>
        ///     Loads the plugin.
        /// </summary>
        /// <param name="pluginLocation">The plugin location.</param>
        /// <returns>An Assembly</returns>
        private static Assembly LoadPlugin(string pluginLocation)
        {
            var loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyPath(pluginLocation);
        }
    }
}