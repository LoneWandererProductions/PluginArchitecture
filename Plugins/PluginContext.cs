/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        PluginContext.cs
 * PURPOSE:     Simple Sane Implementation of a Managed Plugin Context.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

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