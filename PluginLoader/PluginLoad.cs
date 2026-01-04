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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Plugins.Interfaces;

namespace PluginLoader
{
    public static class PluginLoad
    {
        public static IReadOnlyList<IPlugin> LoadAll(string baseDirectory)
        {
            if (!Directory.Exists(baseDirectory))
                return Array.Empty<IPlugin>();

            var plugins = new List<IPlugin>();

            // 1) Prefer marker files if present
            var markerFiles = Directory.EnumerateFiles(baseDirectory, "*.plugin").ToList();

            if (markerFiles.Count > 0)
            {
                foreach (var marker in markerFiles)
                {
                    var dll = Path.ChangeExtension(marker, ".dll");
                    TryLoadFromDll(dll, plugins);
                }
            }
            else
            {
                // 2) Fallback: load all dlls
                foreach (var dll in Directory.EnumerateFiles(baseDirectory, "*.dll"))
                {
                    TryLoadFromDll(dll, plugins);
                }
            }

            return plugins;
        }

        private static void TryLoadFromDll(string dllPath, List<IPlugin> plugins)
        {
            if (!File.Exists(dllPath))
                return;

            try
            {
                var assembly = Assembly.LoadFrom(dllPath);

                var pluginTypes = assembly.GetTypes()
                    .Where(t =>
                        !t.IsAbstract &&
                        typeof(IPlugin).IsAssignableFrom(t));

                foreach (var type in pluginTypes)
                {
                    if (Activator.CreateInstance(type) is IPlugin plugin)
                        plugins.Add(plugin);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Trace.WriteLine($"Plugin load error: {dllPath}");
                foreach (var loaderEx in ex.LoaderExceptions)
                    Debug.WriteLine(loaderEx);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Plugin load error: {dllPath}\n{ex}");
            }
        }
    }
}
