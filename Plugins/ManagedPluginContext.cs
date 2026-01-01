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

            // Optional: initialize to default values if you want
            for (int i = 0; i < _variables.Length; i++)
                _variables[i] = GetDefault(variableSymbols[i].Type);

            for (int i = 0; i < _results.Length; i++)
                _results[i] = GetDefault(resultSymbols[i].Type);
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
    }
}