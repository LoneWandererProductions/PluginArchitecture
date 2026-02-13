/*
* COPYRIGHT:   See COPYING in the top level directory
* PROJECT:     Plugin
* FILE:        Main/MainWindow.xaml.cs
* PURPOSE:     MainWindow, just for showcasing the PluginController User control.
* PROGRAMER:   Peter Geinitz (Wayfarer)
*/

using PluginLoader;
using Plugins.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Main
{
    /// <summary>
    /// Test Windows application to load and display plugins.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : INotifyPropertyChanged
    {
        /// <summary>
        /// The plugins
        /// </summary>
        private ObservableCollection<IPlugin> _plugins = new();

        public ObservableCollection<IPlugin> Plugins
        {
            get => _plugins;
            set
            {
                if (ReferenceEquals(_plugins, value))
                    return;

                _plugins = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins");
            var plugins = PluginLoad.LoadAll(pluginPath);
            // simply set the raw IPlugin collection
            PluginControl.SetPlugins(plugins);
        }
    }
}