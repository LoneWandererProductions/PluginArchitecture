/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        PluginSymbolViewModel.cs
 * PURPOSE:     The ViewModel for a single Plugin Symbol and its interaction logic.
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
    /// <summary>
    /// ViewModel for a single <see cref="SymbolDefinition"/> and its interaction logic.
    /// Acts as the MVVM bridge between plugin metadata, runtime context, and UI binding.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public sealed class PluginSymbolViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the owning plugin instance.
        /// </summary>
        public IPlugin Plugin { get; }

        /// <summary>
        /// Gets the symbol definition metadata.
        /// </summary>
        public SymbolDefinition Definition { get; }

        /// <summary>
        /// Gets or sets the active plugin execution context.
        /// May be <c>null</c> if the symbol is not yet bound.
        /// </summary>
        public IPluginContext? Context { get; set; }

        /// <summary>
        /// Gets the symbol index inside the plugin.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the execute command for method symbols.
        /// Null for non-method symbols.
        /// </summary>
        public ICommand? ExecuteCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSymbolViewModel"/> class.
        /// </summary>
        /// <param name="plugin">The plugin instance.</param>
        /// <param name="symbol">The symbol definition.</param>
        /// <param name="index">The symbol index.</param>
        /// <param name="context">The plugin context.</param>
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

        /// <summary>
        /// Raises a property changed notification.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Updates the plugin context.
        /// </summary>
        /// <param name="context">The new context.</param>
        public void SetContext(IPluginContext context)
        {
            Context = context;
            OnPropertyChanged(nameof(Value));
        }

        /// <summary>
        /// Gets a value indicating whether this symbol represents a method.
        /// </summary>
        public bool IsMethod => Definition.Kind == SymbolType.Method;

        /// <summary>
        /// Gets a value indicating whether this symbol represents data.
        /// </summary>
        public bool IsData => Definition.Kind == SymbolType.Data;

        /// <summary>
        /// Gets a value indicating whether the data symbol is editable.
        /// </summary>
        public bool IsEditable => IsData && Definition.Direction != DirectionType.Output;

        /// <summary>
        /// Gets the symbol identifier.
        /// </summary>
        public int Id => Definition.Id;

        /// <summary>
        /// Gets the symbol name.
        /// </summary>
        public string Name => Definition.Name;

        /// <summary>
        /// Gets the symbol description.
        /// </summary>
        public string? Description => Definition.Description;

        /// <summary>
        /// Gets the symbol type.
        /// </summary>
        public Type Type => Definition.Type;

        /// <summary>
        /// Gets the symbol direction.
        /// </summary>
        public DirectionType Direction => Definition.Direction;

        /// <summary>
        /// Gets the declared size of the symbol.
        /// </summary>
        public int? Size => Definition.Size;

        /// <summary>
        /// Gets the effective size of the symbol.
        /// </summary>
        public int EffectiveSize => Definition.EffectiveSize;

        /// <summary>
        /// Gets the symbol kind.
        /// </summary>
        public SymbolType Kind => Definition.Kind;

        /// <summary>
        /// Gets or sets the symbol value through the current context.
        /// Supports managed and unmanaged plugin contexts.
        /// </summary>
        public object? Value
        {
            get
            {
                if (!IsData || Context == null)
                    return null;

                return Context switch
                {
                    IManagedPluginContext m => m.GetVariable<object>(Index),
                    IUnmanagedPluginContext u => GetUnmanagedValue(u),
                    _ => null
                };
            }
            set
            {
                if (!IsEditable || Context == null)
                    return;

                switch (Context)
                {
                    case IManagedPluginContext m:
                        m.SetVariable(Index, value);
                        break;

                    case IUnmanagedPluginContext u:
                        SetUnmanagedValue(u, value);
                        break;
                }

                OnPropertyChanged(nameof(Value));
            }
        }

        /// <summary>
        /// Sets a value in an unmanaged context with type conversion.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NotSupportedException">Unmanaged type '{type}' is not supported.</exception>
        private void SetUnmanagedValue(IUnmanagedPluginContext context, object? value)
        {
            if (value == null)
                return;

            var type = Definition.Type;

            if (type == typeof(int))
                context.SetVariable(Index, Convert.ToInt32(value));
            else if (type == typeof(float))
                context.SetVariable(Index, Convert.ToSingle(value));
            else if (type == typeof(double))
                context.SetVariable(Index, Convert.ToDouble(value));
            else if (type == typeof(bool))
                context.SetVariable(Index, Convert.ToBoolean(value));
            else if (type == typeof(long))
                context.SetVariable(Index, Convert.ToInt64(value));
            else if (type == typeof(short))
                context.SetVariable(Index, Convert.ToInt16(value));
            else if (type == typeof(byte))
                context.SetVariable(Index, Convert.ToByte(value));
            else
                throw new NotSupportedException($"Unmanaged type '{type}' is not supported.");
        }

        /// <summary>
        /// Reads a value from an unmanaged context with proper typing.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Unmanaged type '{type}' is not supported.</exception>
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

            throw new NotSupportedException($"Unmanaged type '{type}' is not supported.");
        }
    }
}
