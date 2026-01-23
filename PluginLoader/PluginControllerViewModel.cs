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
using System;
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
        /// Gets all messages produced by plugins.
        /// </summary>
        public string PluginMessages
        {
            get => _pluginMessages;
            set
            {
                if (_pluginMessages == value)
                    return;

                _pluginMessages = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the preferred context.
        /// </summary>
        /// <value>
        /// The preferred context.
        /// </value>
        public PluginContextSupport PreferredContext { get; set; }
    = PluginContextSupport.Managed;


        /// <summary>
        /// The plugin messages
        /// </summary>
        private string _pluginMessages = string.Empty;

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
        /// Sets the plugins and creates an appropriate context for each.
        /// Also subscribes to ResultChanged events if the context supports it.
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

                    // Determine which context to use
                    PluginContextSupport chosenContext = PreferredContext;

                    // If preferred context is not supported, pick any supported one
                    if (!plugin.SupportedContexts.HasFlag(chosenContext))
                    {
                        if (plugin.SupportedContexts.HasFlag(PluginContextSupport.ManagedCom))
                            chosenContext = PluginContextSupport.ManagedCom;
                        else if (plugin.SupportedContexts.HasFlag(PluginContextSupport.Managed))
                            chosenContext = PluginContextSupport.Managed;
                        else if (plugin.SupportedContexts.HasFlag(PluginContextSupport.Unmanaged))
                            chosenContext = PluginContextSupport.Unmanaged;
                        else
                            throw new InvalidOperationException(
                                $"Plugin {plugin.Name} does not support any known context types.");
                    }

                    // Create the actual context instance
                    context = chosenContext switch
                    {
                        PluginContextSupport.ManagedCom => new ManagedPluginContextCom(symbols),
                        PluginContextSupport.Managed => new ManagedPluginContext(symbols),
                        PluginContextSupport.Unmanaged => new UnmanagedPluginContext(symbols),
                        _ => throw new InvalidOperationException("Unsupported PluginContextSupport type.")
                    };

                    // Subscribe to result changes if context supports IPluginCommunicator
                    if (context is IPluginCommunicator communicator)
                    {
                        communicator.ResultChanged += OnPluginResultChanged;
                    }

                    // Initialize plugin with chosen context
                    plugin.Initialize(context);
                }

                // Pass both plugin and context to the view model
                Plugins.Add(new PluginViewModel(plugin));
            }
        }

        /// <summary>
        /// Called when [plugin result changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ResultChangedEventArgs"/> instance containing the event data.</param>
        private void OnPluginResultChanged(object? sender, ResultChangedEventArgs e)
        {
            PluginMessages += $"[{DateTime.Now:HH:mm:ss}] {e.Name}: {e.Value}{Environment.NewLine}";
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