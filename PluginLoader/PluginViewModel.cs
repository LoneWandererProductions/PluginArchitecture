/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        PluginViewModel.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using Plugins.Interfaces;
using System.Collections.ObjectModel;

namespace PluginLoader
{
    public sealed class PluginViewModel
    {
        /// <summary>
        /// The raw plugin instance.
        /// Needed for execution and context access.
        /// </summary>
        public IPlugin Command { get; }

        public string Name { get; }
        public string Version { get; }
        public string Description { get; }

        public IPluginContext Context { get; }

        /// <summary>
        /// All symbols exposed by this plugin (if any).
        /// </summary>
        public ObservableCollection<PluginSymbolViewModel> Symbols { get; } = new();

        public PluginViewModel(IPlugin plugin)
        {
            Command = plugin;

            Name = plugin.Name;
            Version = plugin.Version;
            Description = plugin.Description;
            Context = plugin.Context;

            LoadSymbols();
        }

        private void LoadSymbols()
        {
            Symbols.Clear();

            if (Command is not ISymbolProvider provider)
                return;

            int index = 0;
            foreach (SymbolDefinition symbol in provider.GetSymbols())
            {
                Symbols.Add(new PluginSymbolViewModel(
                    plugin: Command, // the plugin instance this ViewModel wraps
                    symbol: symbol,
                    index: index++,
                    context: Context
                ));
            }
        }
    }
}