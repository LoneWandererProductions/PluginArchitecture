using System.Collections.Generic;
using System.Windows;

namespace Main
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dct = new Dictionary<int, object>();
            PlugController.SetEnvironmentVariables(dct);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //TODO test
            var item = PlugController.ObservablePlugin[1];

            //TODO add check if command has return value
            item.Command.ExecuteCommand(0);

            //TODO display Return Value
            // check if result has correct id, if result is new and if result is from the right Command call!
            item.Command.GetValue(0);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PlugController.UpdateEnvironmentVariables(0, new object());
        }

        private void button_Click_3(object sender, RoutedEventArgs e)
        {
            var item = PlugController.ObservablePlugin[0];

            item.Command.Execute();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var item = PlugController.ObservablePlugin[1];

            item.Command.Execute();
        }
    }
}