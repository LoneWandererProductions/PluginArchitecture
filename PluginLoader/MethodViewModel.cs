using Plugins;
using Plugins.Interfaces;
using System.Windows.Input;
using ViewModel;

namespace PluginLoader
{
    public sealed class MethodViewModel
    {
        public string Name { get; }
        public ICommand ExecuteCommand { get; }

        public MethodViewModel(IPlugin plugin, SymbolDefinition def, int id)
        {
            Name = def.Name;

            ExecuteCommand = new RelayCommand(() =>
            {
                plugin.Execute(id);
            });
        }
    }
}