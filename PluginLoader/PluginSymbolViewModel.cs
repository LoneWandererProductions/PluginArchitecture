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
    public sealed class PluginSymbolViewModel : ViewModelBase
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
        public IPluginContext? Context
        {
            get => _context;
            private set =>
                SetPropertyAndCallback(ref _context, value, _ => { RaisePropertyChangedFor(nameof(Value)); });
        }

        /// <summary>
        /// The symbol ID (always from SymbolDefinition)
        /// </summary>
        public int Id => Definition.Id;

        /// <summary>
        /// Gets the index of the context (for data symbols only)
        /// </summary>
        /// <value>
        /// The index of the context.
        /// </value>
        public int? ContextIndex { get; }

        /// <summary>
        /// Gets the execute command for method symbols.
        /// Null for non-method symbols.
        /// </summary>
        public ICommand? ExecuteCommand { get; }

        /// <summary>
        /// The context
        /// </summary>
        private IPluginContext? _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSymbolViewModel"/> class.
        /// </summary>
        /// <param name="plugin">The plugin instance.</param>
        /// <param name="symbol">The symbol definition.</param>
        /// <param name="contextIndex">The symbol index in the context (null for methods).</param>
        /// <param name="context">The plugin context.</param>
        public PluginSymbolViewModel(
            IPlugin plugin,
            SymbolDefinition symbol,
            int? contextIndex,
            IPluginContext context)
        {
            Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
            Definition = symbol ?? throw new ArgumentNullException(nameof(symbol));
            ContextIndex = contextIndex;
            Context = context ?? throw new ArgumentNullException(nameof(context));

            // Only methods have an execute command
            if (IsMethod)
            {
                ExecuteCommand = new RelayCommand(() =>
                {
                    plugin.Execute(Id); // Use Id for methods, not ContextIndex
                    OnPropertyChanged(nameof(Value));
                });
            }
        }

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
                if (!IsData || Context == null || ContextIndex == null)
                    return null;

                return Context switch
                {
                    IManagedPluginContext m => m.GetVariable<object>(ContextIndex.Value),
                    IUnmanagedPluginContext u => GetUnmanagedValue(u, ContextIndex.Value),
                    _ => null
                };
            }
            set
            {
                if (!IsEditable || Context == null || ContextIndex == null)
                    return;

                switch (Context)
                {
                    case IManagedPluginContext m:
                        m.SetVariable(ContextIndex.Value, value);
                        break;

                    case IUnmanagedPluginContext u:
                        SetUnmanagedValue(u, ContextIndex.Value, value);
                        break;
                }

                OnPropertyChanged(nameof(Value));
            }
        }

        /// <summary>
        /// Sets a value in an unmanaged context with type conversion.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="index">The variable index.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NotSupportedException">Unmanaged type '{type}' is not supported.</exception>
        private void SetUnmanagedValue(IUnmanagedPluginContext context, int index, object? value)
        {
            if (value == null)
                return;

            var type = Definition.Type;

            if (type == typeof(int))
                context.SetVariable(index, Convert.ToInt32(value));
            else if (type == typeof(float))
                context.SetVariable(index, Convert.ToSingle(value));
            else if (type == typeof(double))
                context.SetVariable(index, Convert.ToDouble(value));
            else if (type == typeof(bool))
                context.SetVariable(index, Convert.ToBoolean(value));
            else if (type == typeof(long))
                context.SetVariable(index, Convert.ToInt64(value));
            else if (type == typeof(short))
                context.SetVariable(index, Convert.ToInt16(value));
            else if (type == typeof(byte))
                context.SetVariable(index, Convert.ToByte(value));
            else
                throw new NotSupportedException($"Unmanaged type '{type}' is not supported.");
        }

        /// <summary>
        /// Reads a value from an unmanaged context with proper typing.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="index">The variable index.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Unmanaged type '{type}' is not supported.</exception>
        private object? GetUnmanagedValue(IUnmanagedPluginContext context, int index)
        {
            var type = Definition.Type;

            if (type == typeof(int))
                return context.GetVariable<int>(index);
            if (type == typeof(float))
                return context.GetVariable<float>(index);
            if (type == typeof(double))
                return context.GetVariable<double>(index);
            if (type == typeof(bool))
                return context.GetVariable<bool>(index);
            if (type == typeof(long))
                return context.GetVariable<long>(index);
            if (type == typeof(short))
                return context.GetVariable<short>(index);
            if (type == typeof(byte))
                return context.GetVariable<byte>(index);

            throw new NotSupportedException($"Unmanaged type '{type}' is not supported.");
        }
    }
}