/*
* COPYRIGHT:   See COPYING in the top level directory
* PROJECT:     Plugin
* FILE:        Main/MainWindow.xaml.cs
* PURPOSE:     MainWindow
* PROGRAMER:   Peter Geinitz (Wayfarer)
*/

using PluginLoader;
using Plugins.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Main
{
    public partial class MainWindow
    {
        public ObservableCollection<IPlugin> Plugins { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Plugins.Clear();

            var pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins");
            PluginLoad.LoadAll(pluginPath);

            foreach (var plugin in PluginLoad.PluginContainer)
                Plugins.Add(plugin);
        }
    }
}