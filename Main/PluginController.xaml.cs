﻿/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     CommonControls
 * FILE:        CommonControls/PluginController.xaml.cs
 * PURPOSE:     Plugin Control, that displays all plugins
 * PROGRAMER:   Peter Geinitz (Wayfarer)
 */

// ReSharper disable MemberCanBeInternal
// ReSharper disable UnusedMethodReturnValue.Global

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Plugin;
using PluginLoader;

namespace Main
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     Plugin Manager
    /// </summary>
    public sealed partial class PluginController : INotifyPropertyChanged
    {
        /// <summary>
        ///     The plugin path
        /// </summary>
        public static readonly DependencyProperty TargetPath = DependencyProperty.Register(nameof(TargetPath),
            typeof(string),
            typeof(PluginController), null);

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="PluginController" /> class.
        /// </summary>
        public PluginController()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the plugin path.
        /// </summary>
        /// <value>
        ///     The plugin path.
        /// </value>
        public string PluginPath
        {
            get => (string) GetValue(TargetPath);
            set
            {
                SetValue(TargetPath, value);
                Initiate();
            }
        }

        /// <summary>
        ///     Gets or sets the observable plugin.
        /// </summary>
        /// <value>
        ///     The observable plugin.
        /// </value>
        public ObservableCollection<PluginItem> ObservablePlugin { get; set; } = new();

        /// <inheritdoc />
        /// <summary>
        ///     Tells the Components something was changed
        ///     Needed since we have to trigger it user defined
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     The notify property changed.
        /// </summary>
        private void NotifyPropertyChanged()
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(nameof(ObservablePlugin)));
        }

        /// <summary>
        ///     Initiates this instance.
        /// </summary>
        private void Initiate()
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, PluginPath);
            if (!Directory.Exists(path)) return;

            var check = PluginLoad.LoadAll(path);

            if (!check || PluginLoad.PluginContainer == null || PluginLoad.PluginContainer.Count == 0)
            {
                Trace.WriteLine("No Plugins found.");
                return;
            }

            var lst = new ObservableCollection<PluginItem>();

            foreach (var item in PluginLoad.PluginContainer.Select(plugin => new PluginItem
            {
                Command = plugin, Name = plugin.Name, Version = plugin.Version, Type = plugin.Type
            }))
                lst.Add(item);

            ObservablePlugin = new ObservableCollection<PluginItem>(lst);

            NotifyPropertyChanged();
        }

        /// <summary>
        ///     Sets the environment variables in the plugin Loader
        /// </summary>
        /// <param name="store">
        ///     Sets the environment variables of the base module
        ///     The idea is, the main module has documented Environment Variables, that the plugins can use.
        ///     / This method sets the these Variables.
        /// </param>
        public void SetEnvironmentVariables(Dictionary<int, object> store)
        {
            var check = PluginLoad.SetEnvironmentVariables(store);
            if (!check) Trace.WriteLine("Error, could not set Environment Variable.");
        }

        /// <summary>
        ///     Updates the environment variables.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="data">The data.</param>
        /// <returns>Success Status</returns>
        public bool UpdateEnvironmentVariables(int id, object data)
        {
            if (DataRegister.Store.ContainsKey(id))
                //update specified Environment Variable
                DataRegister.Store[id] = data;

            else
                DataRegister.Store.Add(id, data);

            return true;
        }


        /// <summary>
        ///     Handles the MouseDoubleClick event of the DataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = DataGrid.SelectedItem;
            if (selectedItem is not PluginItem item) return;

            var exe = item.Command.Execute();

            Trace.WriteLine(exe);
        }
    }
}