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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ViewModel;

namespace PluginLoader
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     Plugin Manager
    /// </summary>
    public sealed partial class PluginController
    {
        public PluginController()
        {
            InitializeComponent();
            DataContext = new PluginControllerViewModel();
        }
    }
}
