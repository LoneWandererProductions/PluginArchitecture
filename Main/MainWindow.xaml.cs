using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Main
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
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
            //TODO test
            var item = PlugController.ObservablePlugin[1];

            //TODO add check if command has return value
            var result = item.Command.ExecuteCommand(0);

            var dct = new Dictionary<int, object>();

            PlugController.SetEnvironmentVariables(dct);

            //TODO display Return Value
        }

        private void updateEnvironment_Click(object sender, RoutedEventArgs e)
        {
            PlugController.UpdateEnvironmentVariables(2, "test");
        }

        private void window_Click(object sender, RoutedEventArgs e)
        {
            var item = PlugController.ObservablePlugin[0];

            item.Command.ExecuteCommand(0);
        }
    }
}