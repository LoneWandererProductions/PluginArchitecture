using Plugins;
using Plugins.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PluginLoader
{
    public sealed class PluginControllerViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PluginViewModel> Plugins { get; } = new();
        public ObservableCollection<PluginSymbolViewModel> Symbols { get; } = new();

        private PluginViewModel? _selectedPlugin;

        public PluginViewModel? SelectedPlugin
        {
            get => _selectedPlugin;
            set
            {
                if (_selectedPlugin == value)
                    return;

                _selectedPlugin = value;
                OnPropertyChanged();
                LoadSymbols();
            }
        }

        private PluginSymbolViewModel? _selectedSymbol;
        public PluginSymbolViewModel? SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                if (_selectedSymbol == value)
                    return;

                _selectedSymbol = value;
                OnPropertyChanged();
            }
        }

        public PluginControllerViewModel()
        {
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            Plugins.Clear();

            if (PluginLoad.PluginContainer == null)
                return;

            foreach (var plugin in PluginLoad.PluginContainer)
            {
                Plugins.Add(new PluginViewModel(plugin));
            }
        }

        private void LoadSymbols()
        {
            Symbols.Clear();
            SelectedSymbol = null;

            if (SelectedPlugin?.Command is not ISymbolProvider provider)
                return;

            int index = 0;

            foreach (SymbolDefinition symbol in provider.GetSymbols())
            {
                Symbols.Add(new PluginSymbolViewModel(
                    plugin: SelectedPlugin.Command,
                    symbol: symbol,
                    index: index++,
                    context: SelectedPlugin.Context));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
