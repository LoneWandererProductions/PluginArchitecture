/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        PluginController.xaml.cs
 * PURPOSE:     Plugin Control, that displays all plugins
 * PROGRAMER:   Peter Geinitz (Wayfarer)
 */

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

using Plugins.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace PluginLoader
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     Plugin Manager
    /// </summary>
    public sealed partial class PluginController
    {
        /// <summary>
        /// The vm
        /// </summary>
        private readonly PluginControllerViewModel _vm;

        /// <summary>
        /// The plugins property
        /// </summary>
        public static readonly DependencyProperty PluginsProperty =
            DependencyProperty.Register(
                nameof(Plugins),
                typeof(IEnumerable<IPlugin>),
                typeof(PluginController),
                new PropertyMetadata(null, OnPluginsChanged));


        /// <summary>
        /// The plugin path property
        /// </summary>
        public static readonly DependencyProperty PluginPathProperty =
            DependencyProperty.Register(
                nameof(PluginPath),
                typeof(string),
                typeof(PluginController),
                new PropertyMetadata(null, OnPluginPathChanged));


        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        /// <value>
        /// The plugins.
        /// </value>
        public IEnumerable<IPlugin> Plugins
        {
            get => (IEnumerable<IPlugin>)GetValue(PluginsProperty);
            set => SetValue(PluginsProperty, value);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PluginController"/> class.
        /// </summary>
        public PluginController()
        {
            InitializeComponent();
            _vm = new PluginControllerViewModel();
            DataContext = _vm;
        }

        /// <summary>
        /// Sets the plugins.
        /// </summary>
        /// <param name="plugins">The plugins.</param>
        public void SetPlugins(IEnumerable<IPlugin> plugins)
        {
            _vm.SetPlugins(plugins);
        }

        /// <summary>
        /// Gets or sets the plugin path.
        /// </summary>
        /// <value>
        /// The plugin path.
        /// </value>
        public string? PluginPath
        {
            get => (string?)GetValue(PluginPathProperty);
            set => SetValue(PluginPathProperty, value);
        }

        private static void OnPluginsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PluginController { DataContext: PluginControllerViewModel vm } &&
                e.NewValue is IEnumerable<IPlugin> plugins)
            {
                vm.SetPlugins(plugins);
            }
        }

        /// <summary>
        /// Called when [plugin path changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private static void OnPluginPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PluginController control)
                return;

            if (e.NewValue is not string path || string.IsNullOrWhiteSpace(path))
                return;

            var plugins = PluginLoad.LoadAll(path);
            control._vm.SetPlugins(plugins);
        }
    }
}