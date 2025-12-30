using System.Windows.Input;

namespace PluginLoader
{
    public sealed class MethodSymbolViewModel
    {
        public string Name { get; set; } = string.Empty;
        public ICommand? ExecuteCommand { get; set; }
    }
}