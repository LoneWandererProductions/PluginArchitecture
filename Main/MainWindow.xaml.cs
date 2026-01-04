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
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Main
{
    public partial class MainWindow : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins");
            var plugins = PluginLoad.LoadAll(pluginPath);
            // simply set the raw IPlugin collection
            PluginControl.Plugins = plugins;
        }
    }
}