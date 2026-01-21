/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        PluginContext.cs
 * PURPOSE:     Simple Sane Implementation of a Managed Plugin Context.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;
using Plugins.Interfaces;

namespace Plugins
{
    /// <inheritdoc />
    /// <summary>
    /// Sample Implementation of a Managed Plugin Context.
    /// </summary>
    /// <seealso cref="Plugins.Interfaces.IManagedPluginContext" />
    public sealed class ManagedPluginContext : IManagedPluginContext
    {
        /// <summary>
        /// The variables
        /// </summary>
        private readonly object[] _variables;

        /// <summary>
        /// The results
        /// </summary>
        private readonly object[] _results;

        /// <summary>
        /// Lookup table for variable symbols (name -> index)
        /// </summary>
        private readonly Dictionary<string, int> _variableLookup = new();

        /// <summary>
        /// Lookup table for result symbols (name -> index)
        /// </summary>
        private readonly Dictionary<string, int> _resultLookup = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedPluginContext"/> class.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <exception cref="System.ArgumentNullException">symbols</exception>
        public ManagedPluginContext(IReadOnlyList<SymbolDefinition> symbols)
        {
            if (symbols == null)
                throw new ArgumentNullException(nameof(symbols));

            // Only consider Data symbols for memory / context
            var dataSymbols = symbols
                .Where(s => s.Kind == SymbolType.Data)
                .ToList();

            // Split variables vs results based on Direction
            var variableSymbols = dataSymbols
                .Where(s => s.Direction == DirectionType.Input || s.Direction == DirectionType.InOut)
                .ToList();

            var resultSymbols = dataSymbols
                .Where(s => s.Direction == DirectionType.Output || s.Direction == DirectionType.InOut)
                .ToList();

            // Create arrays with the correct length
            _variables = new object[variableSymbols.Count];
            _results = new object[resultSymbols.Count];

            // Build lookup tables
            for (int i = 0; i < variableSymbols.Count; i++)
                _variableLookup[variableSymbols[i].Name] = i;

            for (int i = 0; i < resultSymbols.Count; i++)
                _resultLookup[resultSymbols[i].Name] = i;

            // Initialize defaults
            for (int i = 0; i < _variables.Length; i++)
                _variables[i] = GetDefault(variableSymbols[i].Type);

            for (int i = 0; i < _results.Length; i++)
                _results[i] = GetDefault(resultSymbols[i].Type);
        }

        /// <inheritdoc />
        public int Find(string name)
        {
            bool hasVar = _variableLookup.ContainsKey(name);
            bool hasRes = _resultLookup.ContainsKey(name);

            if (hasVar && hasRes)
                throw new InvalidOperationException(
                    $"Symbol '{name}' exists as both Variable and Result.");

            if (hasVar)
                return FindVariable(name);

            if (hasRes)
                return FindResult(name);

            throw new KeyNotFoundException($"Symbol '{name}' not found.");
        }

        /// <inheritdoc />
        public int Find(SymbolDefinition symbol)
        {
            if (symbol.Kind != SymbolType.Data)
                throw new InvalidOperationException(
                    $"Symbol '{symbol.Name}' of kind '{symbol.Kind}' is not stored in context.");

            return symbol.Direction == DirectionType.Output
                ? FindResult(symbol.Name)
                : FindVariable(symbol.Name);
        }

        /// <inheritdoc />
        public int FindVariable(string name)
        {
            if (!_variableLookup.TryGetValue(name, out var index))
                throw new KeyNotFoundException($"Variable symbol '{name}' not found.");

            return index;
        }

        /// <inheritdoc />
        public int FindResult(string name)
        {
            if (!_resultLookup.TryGetValue(name, out var index))
                throw new KeyNotFoundException($"Result symbol '{name}' not found.");

            return index;
        }

        /// <summary>
        /// Helper to get default value for a Type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Default value of a type</returns>
        private static object? GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <inheritdoc />
        public int VariableCount => _variables.Length;

        /// <inheritdoc />
        public int ResultCount => _results.Length;

        /// <inheritdoc />
        public T GetVariable<T>(int index) => (T)_variables[index];

        /// <inheritdoc />
        public void SetVariable<T>(int index, T value) => _variables[index] = value;

        /// <inheritdoc />
        public T GetResult<T>(int index) => (T)_results[index];

        /// <inheritdoc />
        public void SetResult<T>(int index, T value) => _results[index] = value;

        /// <inheritdoc />
        /// <summary>
        /// Returns a string representation of the current context,
        /// showing variables and results by name and value.
        /// </summary>
        public override string ToString()
        {
            return $"ManagedPluginContext.";
        }

    }
}
