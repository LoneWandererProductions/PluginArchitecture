using System;
using System.Collections.Generic;
using System.IO;
using System.Windows; /*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugin
 * FILE:        Main/MainWindow.xaml.cs
 * PURPOSE:     MainWindow
 * PROGRAMER:   Peter Geinitz (Wayfarer)
 */

namespace Main
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void initiate_Click(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var obj = DateTime.Now;
            var dct = new Dictionary<int, object> {{0, path}, {1, obj}};

            PlugController.SetEnvironmentVariables(dct);

            foreach (var item in PlugController.ObservablePlugin) item.Command.Execute();
        }

        private void console_Click(object sender, RoutedEventArgs e)
        {
            var item = PlugController.ObservablePlugin[0];

            var result = item.Command.ExecuteCommand(0);

            txtOutput.Text = string.Empty;
            txtOutput.Text = result.ToString() ?? string.Empty;
        }

        private void updateEnvironment_Click(object sender, RoutedEventArgs e)
        {
            PlugController.UpdateEnvironmentVariables(0, "test");
        }

        private void window_Click(object sender, RoutedEventArgs e)
        {
            var item = PlugController.ObservablePlugin[1];

            item.Command.ExecuteCommand(0);
        }
    }
}