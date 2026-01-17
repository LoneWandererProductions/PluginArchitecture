/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        PluginSymbolViewModel.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using Plugins.Enums;
using Plugins.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Input;
using ViewModel;

namespace PluginLoader
{
    public sealed class PluginSymbolViewModel : INotifyPropertyChanged
    {
        public IPlugin Plugin { get; }
        public SymbolDefinition Definition { get; }
        public IPluginContext? Context { get; set; } // nullable

        public int Index { get; }

        public ICommand? ExecuteCommand { get; }


        public PluginSymbolViewModel(IPlugin plugin, SymbolDefinition symbol, int index, IPluginContext context)
        {
            Plugin = plugin;
            Definition = symbol;
            Index = index;
            Context = context;

            if (IsMethod)
            {
                ExecuteCommand = new RelayCommand(() =>
                {
                    plugin.Execute(index);
                    OnPropertyChanged(nameof(Value));
                });
            }
        }

        private void OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void SetUnmanagedValue(IUnmanagedPluginContext context, object? value)
        {
            if (value == null)
                return;

            var type = Definition.Type;

            if (type == typeof(int))
            {
                context.SetVariable(Index, Convert.ToInt32(value));
                return;
            }

            if (type == typeof(float))
            {
                context.SetVariable(Index, Convert.ToSingle(value));
                return;
            }

            if (type == typeof(double))
            {
                context.SetVariable(Index, Convert.ToDouble(value));
                return;
            }

            if (type == typeof(bool))
            {
                context.SetVariable(Index, Convert.ToBoolean(value));
                return;
            }

            if (type == typeof(long))
            {
                context.SetVariable(Index, Convert.ToInt64(value));
                return;
            }

            if (type == typeof(short))
            {
                context.SetVariable(Index, Convert.ToInt16(value));
                return;
            }

            if (type == typeof(byte))
            {
                context.SetVariable(Index, Convert.ToByte(value));
                return;
            }

            throw new NotSupportedException(
                $"Unmanaged type '{type}' is not supported.");
        }


        private object? GetUnmanagedValue(IUnmanagedPluginContext context)
        {
            var type = Definition.Type;

            if (type == typeof(int))
                return context.GetVariable<int>(Index);

            if (type == typeof(float))
                return context.GetVariable<float>(Index);

            if (type == typeof(double))
                return context.GetVariable<double>(Index);

            if (type == typeof(bool))
                return context.GetVariable<bool>(Index);

            if (type == typeof(long))
                return context.GetVariable<long>(Index);

            if (type == typeof(short))
                return context.GetVariable<short>(Index);

            if (type == typeof(byte))
                return context.GetVariable<byte>(Index);

            throw new NotSupportedException(
                $"Unmanaged type '{type}' is not supported.");
        }

    }
}