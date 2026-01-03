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
        private readonly PluginControllerViewModel _vm;

        public static readonly DependencyProperty PluginsProperty =
            DependencyProperty.Register(
            nameof(Plugins),
            typeof(IEnumerable<IPlugin>),
            typeof(PluginController),
            new PropertyMetadata(null, OnPluginsChanged));

        public IEnumerable<IPlugin> Plugins
        {
            get => (IEnumerable<IPlugin>)GetValue(PluginsProperty);
            set => SetValue(PluginsProperty, value);
        }


        public PluginController()
        {
            InitializeComponent();
            _vm = new PluginControllerViewModel();
            DataContext = _vm;
        }

        public void SetPlugins(IEnumerable<IPlugin> plugins)
        {
            _vm.SetPlugins(plugins);
        }

        private static void OnPluginsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PluginController control &&
                control.DataContext is PluginControllerViewModel vm &&
                e.NewValue is IEnumerable<IPlugin> plugins)
            {
                vm.SetPlugins(plugins);
            }
        }
    }
}