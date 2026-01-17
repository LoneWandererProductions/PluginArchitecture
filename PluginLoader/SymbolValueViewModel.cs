/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        SymbolValueViewModel.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using Plugins.Enums;
using Plugins.Interfaces;
using System;
using System.ComponentModel;

namespace PluginLoader
{
    public sealed class SymbolValueViewModel : INotifyPropertyChanged
    {
        private readonly IPluginContext _context;
        private readonly int _index;

        public string Name { get; }
        public Type DataType { get; }
        public SymbolType SymbolType { get; }
        public DirectionType Direction { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SymbolValueViewModel(
            IPluginContext context,
            SymbolDefinition definition,
            int index)
        {
            _context = context;
            _index = index;

            Name = definition.Name;
            DataType = definition.Type;
            SymbolType = definition.Kind;
            Direction = definition.Direction;
        }

        public object? Value
        {
            get
            {
                return _context switch
                {
                    IManagedPluginContext m =>
                        m.GetVariable<object>(_index),

                    IUnmanagedPluginContext u =>
                        GetUnmanagedValue(u),

                    _ => null
                };
            }
            set
            {
                if (Direction == DirectionType.Output)
                    return;

                switch (_context)
                {
                    case IManagedPluginContext m:
                        m.SetVariable(_index, value);
                        break;

                    case IUnmanagedPluginContext u:
                        SetUnmanagedValue(u, value);
                        break;
                }

                OnPropertyChanged(nameof(Value));
            }
        }

        private object GetUnmanagedValue(IUnmanagedPluginContext ctx)
        {
            if (DataType == typeof(int))
                return ctx.GetVariable<int>(_index);

            if (DataType == typeof(float))
                return ctx.GetVariable<float>(_index);

            if (DataType == typeof(double))
                return ctx.GetVariable<double>(_index);

            if (DataType == typeof(bool))
                return ctx.GetVariable<bool>(_index);

            throw new NotSupportedException(
                $"Unsupported unmanaged type: {DataType}");
        }

        private void SetUnmanagedValue(IUnmanagedPluginContext ctx, object? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (DataType == typeof(int))
                ctx.SetVariable(_index, Convert.ToInt32(value));
            else if (DataType == typeof(float))
                ctx.SetVariable(_index, Convert.ToSingle(value));
            else if (DataType == typeof(double))
                ctx.SetVariable(_index, Convert.ToDouble(value));
            else if (DataType == typeof(bool))
                ctx.SetVariable(_index, Convert.ToBoolean(value));
            else
                throw new NotSupportedException(
                    $"Unsupported unmanaged type: {DataType}");
        }

        public void Refresh() =>
            OnPropertyChanged(nameof(Value));

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}