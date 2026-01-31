/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        PluginViewModel.cs
 * PURPOSE:     Plugin ViewModel for use in the PluginLoader Control.
 *              Tries to expose all relevant information about a plugin via Symbols.
 *              If they are not exposed via <see cref="ISymbolProvider"/>, no symbols are shown.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using Plugins.Enums;
using Plugins.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PluginLoader
{
    /// <summary>
    /// Root ViewModel representing a single plugin instance.
    /// Exposes plugin metadata, execution context and symbol view models
    /// for UI binding and inspection.
    /// </summary>
    public sealed class PluginViewModel
    {
        /// <summary>
        /// Gets the raw plugin instance.
        /// Required for execution and context access.
        /// </summary>
        public IPlugin Plugin { get; }

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the plugin execution context.
        /// </summary>
        public IPluginContext Context { get; }

        /// <summary>
        /// Gets all symbols exposed by this plugin (if any).
        /// Each symbol is wrapped into a <see cref="PluginSymbolViewModel"/>.
        /// </summary>
        public ObservableCollection<PluginSymbolViewModel> Symbols { get; } = new();

        /// <summary>
        /// Gets a value indicating whether this instance has symbols.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has symbols; otherwise, <c>false</c>.
        /// </value>
        public bool HasSymbols => Symbols.Count > 0;

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh() => LoadSymbols();

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <value>
        /// The methods.
        /// </value>
        public IEnumerable<PluginSymbolViewModel> Methods => Symbols.Where(s => s.IsMethod);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IEnumerable<PluginSymbolViewModel> Data => Symbols.Where(s => s.IsData);

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int? Index { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginViewModel"/> class.
        /// </summary>
        /// <param name="plugin">The plugin instance.</param>
        public PluginViewModel(IPlugin plugin)
        {
            Plugin = plugin;

            Name = plugin.Name;
            Version = plugin.Version;
            Description = plugin.Description;
            Context = plugin.Context;

            LoadSymbols();
        }

        /// <summary>
        /// Loads all symbols from the plugin if it implements <see cref="ISymbolProvider"/>.
        /// Clears previous symbols and recreates the symbol view models.
        /// </summary>
        private void LoadSymbols()
        {
            Symbols.Clear();

            if (Plugin is not ISymbolProvider provider)
                return;

            var symbols = provider.GetSymbols();

            foreach (SymbolDefinition symbol in symbols)
            {
                int index = 0;

                if (symbol.Kind == SymbolType.Data)
                    index = Plugin.Context.Find(symbol);

                Symbols.Add(new PluginSymbolViewModel(
                    plugin: Plugin,
                    symbol: symbol,
                    contextIndex: index,
                    context: Context
                ));
            }
        }
    }
}