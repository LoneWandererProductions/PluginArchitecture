using Plugins;
using Plugins.Enums;
using Plugins.Interfaces;
using System;

namespace PluginLoader
{
    public sealed class PluginSymbolViewModel
    {
        public IPlugin Plugin { get; }
        public SymbolDefinition Definition { get; }
        public IPluginContext? Context { get; private set; } // nullable
        public int Index { get; }

        public PluginSymbolViewModel(IPlugin plugin, SymbolDefinition symbol, int index)
        {
            Plugin = plugin;
            Definition = symbol;
            Index = index;
        }

        public void SetContext(IPluginContext context)
        {
            Context = context;
        }

        public bool IsMethod => Definition.Kind == SymbolType.Method;
        public bool IsData => Definition.Kind == SymbolType.Data;
        public bool IsEditable => IsData && Definition.Direction != DirectionType.Output;

        public object? Value
        {
            get
            {
                if (!IsData || Context == null) return null;

                return Context switch
                {
                    IManagedPluginContext m => m.GetVariable<object>(Index),
                    IUnmanagedPluginContext u => GetUnmanagedValue(u),
                    _ => null
                };
            }
            set
            {
                if (!IsEditable || Context == null) return;

                switch (Context)
                {
                    case IManagedPluginContext m:
                        m.SetVariable(Index, value);
                        break;
                    case IUnmanagedPluginContext u:
                        SetUnmanagedValue(u, value);
                        break;
                }
            }
        }

        private static object? GetUnmanagedValue(IUnmanagedPluginContext context)
        {
            // here you can handle int/float/etc based on SymbolDefinition.DataType
            throw new NotImplementedException();
        }

        private static void SetUnmanagedValue(IUnmanagedPluginContext context, object? value)
        {
            // handle unmanaged set
            throw new NotImplementedException();
        }
    }

}