/*
* COPYRIGHT:   See COPYING in the top level directory
* PROJECT:     Plugin
* FILE:        Main/MainWindow.xaml.cs
* PURPOSE:     MainWindow
* PROGRAMER:   Peter Geinitz (Wayfarer)
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

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

        }

        private void console_Click(object sender, RoutedEventArgs e)
        {
            var item = PlugController.ObservablePlugin[0];

            txtOutput.Text = string.Empty;
        }


        private void window_Click(object sender, RoutedEventArgs e)
        {
            var item = PlugController.ObservablePlugin[1];
        }
    }
}