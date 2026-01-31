/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        ManagedPluginContextCom.cs
 * PURPOSE:     Managed plugin context with optional communication support.
 *              Fires events when result values change, allowing subscribers
 *              to react to changes dynamically.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;
using Plugins.Interfaces;

namespace Plugins
{
    /// <summary>
    /// A managed plugin context that implements <see cref="IPluginCommunicator"/>.
    /// Extends <see cref="ManagedPluginContext"/> by notifying listeners
    /// whenever a result value is changed.
    /// </summary>
    /// <seealso cref="ManagedPluginContext"/>
    /// <seealso cref="IPluginCommunicator"/>
    public class ManagedPluginContextCom : ManagedPluginContext, IPluginCommunicator
    {
        /// <summary>
        /// Event fired when a result changes.
        /// </summary>
        public event EventHandler<ResultChangedEventArgs>? ResultChanged;

        /// <summary>
        /// Reverse lookup table for result symbols (index -> name)
        /// </summary>
        private readonly Dictionary<int, string> _resultIndexLookup = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedPluginContextCom"/> class.
        /// </summary>
        /// <param name="symbols">The symbol definitions used to initialize variables and results.</param>
        public ManagedPluginContextCom(IReadOnlyList<SymbolDefinition> symbols)
            : base(symbols)
        {
            if (symbols == null)
                throw new ArgumentNullException(nameof(symbols));

            var resultSymbols = symbols
                .Where(s => s.Kind == SymbolType.Data &&
                            (s.Direction == DirectionType.Output || s.Direction == DirectionType.InOut))
                .ToList();

            for (int i = 0; i < resultSymbols.Count; i++)
                _resultIndexLookup[i] = resultSymbols[i].Name;
        }

        /// <inheritdoc />
        /// <summary>
        /// Notifies subscribers that a result has changed.
        /// </summary>
        /// <param name="name">The name of the result that changed.</param>
        /// <param name="value">The new value of the result.</param>
        protected virtual void NotifyResultChanged(ResultChangedEventArgs args)
        {
            var handler = ResultChanged;
            handler?.Invoke(this, args);
        }

        /// <summary>
        /// Sets a result value and notifies subscribers.
        /// Overrides the base <see cref="ManagedPluginContext.SetResult{T}"/> behavior.
        /// </summary>
        /// <typeparam name="T">Type of the result value.</typeparam>
        /// <param name="index">The index of the result in the context.</param>
        /// <param name="value">The value to set.</param>
        public override void SetResult<T>(int index, T value)
        {
            base.SetResult(index, value);

            string name = GetResultNameByIndex(index);
            RaiseResultChanged(name, index, value);
        }

        /// <summary>
        /// Raises the result changed.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        private void RaiseResultChanged(string name, int index, object? value)
        {
            NotifyResultChanged(new ResultChangedEventArgs(this, name, index, value));
        }

        /// <summary>
        /// Retrieves the result name by index.
        /// Implement this using a reverse lookup table from index to result name.
        /// </summary>
        /// <param name="index">The result index.</param>
        /// <returns>The name of the result at the specified index.</returns>
        /// <exception cref="NotImplementedException">Thrown if not yet implemented.</exception>
        private string GetResultNameByIndex(int index)
        {
            if (_resultIndexLookup.TryGetValue(index, out var name))
                return name;

            return $"Result[{index}]";
        }
    }
}