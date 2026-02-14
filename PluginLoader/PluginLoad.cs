/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugin
 * FILE:        PluginLoad.cs
 * PURPOSE:     Basic Plugin Support, Load all Plugins
 * PROGRAMER:   Peter Geinitz (Wayfarer)
 * SOURCES:     https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support
 */

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

using Loader;
using Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PluginLoader
{
    /// <summary>
    /// Specific Loader for IPlugin implementations
    /// </summary>
    public static class PluginLoad
    {
        /// <summary>
        /// Loads all IPlugin implementations from the specified directory.
        /// Supports optional marker files (*.plugin) to restrict which DLLs are loaded.
        /// </summary>
        public static IReadOnlyList<IPlugin> LoadAll(string baseDirectory)
        {
            if (!Directory.Exists(baseDirectory))
                return Array.Empty<IPlugin>();

            var loader = new PluginLoading();

            // 1) Prefer marker files if present
            var markerFiles = Directory.EnumerateFiles(baseDirectory, "*.plugin");
            if (markerFiles.Any())
            {
                var plugins = new List<IPlugin>();
                foreach (var marker in markerFiles)
                {
                    var dll = Path.ChangeExtension(marker, ".dll");
                    if (File.Exists(dll))
                        plugins.AddRange(loader.Load<IPlugin>(Path.GetDirectoryName(dll) ?? ""));
                }
                return plugins;
            }

            // 2) Fallback: load all DLLs
            return loader.Load<IPlugin>(baseDirectory);
        }
    }
}