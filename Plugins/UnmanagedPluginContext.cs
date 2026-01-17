/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        UnmanagedPluginContext.cs
 * PURPOSE:     An unamanaged plugin context implementation. 
 *              The Idea is to have a memory block only for unmanaged types.
 *              The target is speed and low memory footprint.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;
using Plugins.Interfaces;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Plugins
{
    /// <inheritdoc />
    /// <summary>
    /// Unmanaged plugin context. Only allows unmanaged types. Memory layout is computed automatically from symbols.
    /// </summary>
    public sealed class UnmanagedPluginContext : IUnmanagedPluginContext
    {
        /// <summary>
        /// The memory
        /// </summary>
        private readonly byte[] _memory;

        /// <summary>
        /// The variables
        /// </summary>
        private readonly IReadOnlyList<SymbolDefinition> _variables;

        /// <summary>
        /// The results
        /// </summary>
        private readonly IReadOnlyList<SymbolDefinition> _results;

        /// <summary>
        /// The variable offsets
        /// </summary>
        private readonly int[] _variableOffsets;

        /// <summary>
        /// The result offsets
        /// </summary>
        private readonly int[] _resultOffsets;

        /// <inheritdoc />
        /// <summary>
        /// Gets the variable count.
        /// </summary>
        /// <value>
        /// The variable count.
        /// </value>
        public int VariableCount => _variables.Count;

        /// <inheritdoc />
        /// <summary>
        /// Gets the result count.
        /// </summary>
        /// <value>
        /// The result count.
        /// </value>
        public int ResultCount => _results.Count;

        /// <summary>
        /// Gets the total size.
        /// </summary>
        /// <value>
        /// The total size.
        /// </value>
        public int TotalSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnmanagedPluginContext"/> class.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <exception cref="System.ArgumentNullException">symbols</exception>
        public UnmanagedPluginContext(IReadOnlyList<SymbolDefinition> symbols)
        {
            if (symbols == null) throw new ArgumentNullException(nameof(symbols));

            // Filter only data symbols for memory allocation
            var dataSymbols = symbols
                .Where(s => s.Kind == SymbolType.Data)
                .ToList();

            // Split into variables and results based on direction
            _variables = dataSymbols
                .Where(s => s.Direction == DirectionType.Input || s.Direction == DirectionType.InOut)
                .ToList();

            _results = dataSymbols
                .Where(s => s.Direction == DirectionType.Output || s.Direction == DirectionType.InOut)
                .ToList();

            // Compute offsets
            _variableOffsets = new int[_variables.Count];
            _resultOffsets = new int[_results.Count];

            int offset = 0;
            for (int i = 0; i < _variables.Count; i++)
            {
                _variableOffsets[i] = offset;
                offset += _variables[i].EffectiveSize;
            }

            for (int i = 0; i < _results.Count; i++)
            {
                _resultOffsets[i] = offset;
                offset += _results[i].EffectiveSize;
            }

            TotalSize = offset;
            _memory = new byte[TotalSize];
        }

        /// <inheritdoc />
        public T GetVariable<T>(int index) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateVariableAccess<T>(index);
            return Read<T>(_variableOffsets[index], symbol.EffectiveSize);
        }

        /// <inheritdoc />
        public void SetVariable<T>(int index, T value) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateVariableAccess<T>(index);
            Write(_variableOffsets[index], symbol.EffectiveSize, value);
        }

        /// <inheritdoc />
        public T GetResult<T>(int index) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateResultAccess<T>(index);
            return Read<T>(_resultOffsets[index], symbol.EffectiveSize);
        }

        /// <inheritdoc />
        public void SetResult<T>(int index, T value) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateResultAccess<T>(index);
            Write(_resultOffsets[index], symbol.EffectiveSize, value);
        }

        /// <inheritdoc />
        private T Read<T>(int offset, int size) where T : unmanaged
        {
            var span = _memory.AsSpan(offset, size);
            return MemoryMarshal.Read<T>(span);
        }


        /// <summary>
        /// Writes the specified offset.
        /// </summary>
        /// <typeparam name="T">Generic Datatype, only Primitves.</typeparam>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="value">The value.</param>
        private void Write<T>(int offset, int size, T value) where T : unmanaged
        {
            var span = _memory.AsSpan(offset, size);
            MemoryMarshal.Write(span, ref value);
        }

        /// <summary>
        /// Validates the variable access.
        /// </summary>
        /// <typeparam name="T">Generic Datatype, only Primitves.</typeparam>
        /// <param name="index">The index.</param>
        /// <returns>Verifiy Variable against Symbol.</returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        private SymbolDefinition ValidateVariableAccess<T>(int index)
        {
            if (index < 0 || index >= VariableCount)
                throw new IndexOutOfRangeException();

            var symbol = _variables[index];
            ValidateType<T>(symbol.Type, symbol.EffectiveSize);
            return symbol;
        }

        /// <summary>
        /// Validates the result access.
        /// </summary>
        /// <typeparam name="T">Generic Datatype, only Primitves.</typeparam>
        /// <param name="index">The index.</param>
        /// <returns>Verifiy Resukt against Symbol.</returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        private SymbolDefinition ValidateResultAccess<T>(int index)
        {
            if (index < 0 || index >= ResultCount)
                throw new IndexOutOfRangeException();

            var symbol = _results[index];
            ValidateType<T>(symbol.Type, symbol.EffectiveSize);
            return symbol;
        }

        /// <summary>
        /// Validates the type.
        /// </summary>
        /// <typeparam name="T">Generic Datatype, only Primitves.</typeparam>
        /// <param name="declared">The declared.</param>
        /// <param name="size">The size.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Type mismatch. Declared: {declared}, Requested: {typeof(T)}
        /// or
        /// Size mismatch for {typeof(T)}. Layout: {size}, CLR: {Unsafe.SizeOf<T>()}
        /// </exception>
        private static void ValidateType<T>(Type declared, int size)
        {
            if (declared != typeof(T))
                throw new InvalidOperationException(
                    $"Type mismatch. Declared: {declared}, Requested: {typeof(T)}");

            if (Unsafe.SizeOf<T>() != size)
                throw new InvalidOperationException(
                    $"Size mismatch for {typeof(T)}. Layout: {size}, CLR: {Unsafe.SizeOf<T>()}");
        }

        /// <summary>
        /// Enforces the unmanaged.
        /// </summary>
        /// <returns>Verifiy that Input is possible to be managed as unmanaged.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        private static void EnforceUnmanaged<T>()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                throw new InvalidOperationException(
                    $"{typeof(T)} is not supported in UnmanagedPluginContext");
        }
    }
}