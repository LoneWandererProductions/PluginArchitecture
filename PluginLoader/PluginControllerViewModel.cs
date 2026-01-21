/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        PluginControllerViewModel.cs
 * PURPOSE:     Main View Model and entry point for the PluginLoader Control.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using Plugins.Enums;
using Plugins.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModel;

namespace PluginLoader
{
    /// <summary>
    /// Main ViewModel for the PluginLoader control.
    /// </summary>
    /// <seealso cref="ViewModel.ViewModelBase" />
    public sealed class PluginControllerViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the plugins.
        /// </summary>
        /// <value>
        /// The plugins.
        /// </value>
        public ObservableCollection<PluginViewModel> Plugins { get; } = new();

        /// <summary>
        /// Gets the symbols.
        /// </summary>
        /// <value>
        /// The symbols.
        /// </value>
        public ObservableCollection<PluginSymbolViewModel> Symbols { get; } = new();

        /// <summary>
        /// The selected plugin
        /// </summary>
        private PluginViewModel? _selectedPlugin;

        /// <summary>
        /// Gets or sets the selected plugin.
        /// </summary>
        /// <value>
        /// The selected plugin.
        /// </value>
        public PluginViewModel? SelectedPlugin
        {
            get => _selectedPlugin;
            set
            {
                if (_selectedPlugin == value)
                    return;

                _selectedPlugin = value;
                OnPropertyChanged();
                LoadSymbols();
            }
        }

        private PluginSymbolViewModel? _selectedSymbol;

        /// <summary>
        /// Gets or sets the selected symbol.
        /// </summary>
        /// <value>
        /// The selected symbol.
        /// </value>
        public PluginSymbolViewModel? SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                if (_selectedSymbol == value)
                    return;

                _selectedSymbol = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sets the plugins.
        /// </summary>
        /// <param name="plugins">The plugins.</param>
        public void SetPlugins(IEnumerable<IPlugin> plugins)
        {
            Plugins.Clear();

            foreach (var plugin in plugins)
            {
                IPluginContext? context = null;

                if (plugin is ISymbolProvider provider)
                {
                    var symbols = provider.GetSymbols();
                    context = new ManagedPluginContext(symbols);
                    plugin.Initialize(context);
                }

                Plugins.Add(new PluginViewModel(plugin));
            }
        }

        /// <summary>
        /// Loads the symbols.
        /// </summary>
        private void LoadSymbols()
        {
            Symbols.Clear();
            SelectedSymbol = null;

            if (SelectedPlugin?.Plugin is not ISymbolProvider provider)
                return;

            foreach (SymbolDefinition symbol in provider.GetSymbols())
            {
                int? contextIndex = null;

                if (symbol.Kind == SymbolType.Data)
                    contextIndex = SelectedPlugin.Context.Find(symbol);

                Symbols.Add(new PluginSymbolViewModel(
                    plugin: SelectedPlugin.Plugin,
                    symbol: symbol,
                    contextIndex: contextIndex,
                    context: SelectedPlugin.Context));
            }
        }

    }
}