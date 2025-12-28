using Plugins.Enums;
using Plugins.Interfaces;
using System.Collections.ObjectModel;

namespace PluginLoader
{
    public sealed class PluginContextViewModel
    {
        public ObservableCollection<SymbolValueViewModel> Variables { get; } = new();
        public ObservableCollection<SymbolValueViewModel> Results { get; } = new();
        public ObservableCollection<MethodViewModel> Methods { get; } = new();

        private readonly IPlugin _plugin;

        public PluginContextViewModel(
            IPlugin plugin,
            IPluginContext context,
            ISymbolProvider symbols)
        {
            _plugin = plugin;

            int dataIndex = 0;
            int resultIndex = 0;
            int methodId = 0;

            foreach (var s in symbols.GetSymbols())
            {
                switch (s.Kind)
                {
                    case SymbolType.Data:
                        Variables.Add(new SymbolValueViewModel(context, s, dataIndex++));
                        break;

                    case SymbolType.Method:
                        Methods.Add(new MethodViewModel(plugin, s, methodId++));
                        break;
                }
            }
        }

        public void RefreshResults()
        {
            foreach (var r in Results)
                r.Refresh();
        }
    }
}