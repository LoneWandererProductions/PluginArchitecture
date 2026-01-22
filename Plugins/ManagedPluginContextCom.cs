/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        ManagedPluginContextCom.cs
 * PURPOSE:     Managed plugin context with optional communication support.
 *              Fires events when result values change, allowing subscribers
 *              to react to changes dynamically.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

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
    public sealed class ManagedPluginContextCom : ManagedPluginContext, IPluginCommunicator
    {
        /// <summary>
        /// Event fired when a result changes.
        /// </summary>
        public event Action<string, object?>? ResultChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedPluginContextCom"/> class.
        /// </summary>
        /// <param name="symbols">The symbol definitions used to initialize variables and results.</param>
        public ManagedPluginContextCom(IReadOnlyList<SymbolDefinition> symbols)
            : base(symbols)
        { }

        /// <inheritdoc />
        /// <summary>
        /// Notifies subscribers that a result has changed.
        /// </summary>
        /// <param name="name">The name of the result that changed.</param>
        /// <param name="value">The new value of the result.</param>
        public void NotifyResultChanged(string name, object? value)
        {
            ResultChanged?.Invoke(name, value);
        }

        /// <summary>
        /// Sets a result value and notifies subscribers.
        /// Overrides the base <see cref="ManagedPluginContext.SetResult{T}"/> behavior.
        /// </summary>
        /// <typeparam name="T">Type of the result value.</typeparam>
        /// <param name="index">The index of the result in the context.</param>
        /// <param name="value">The value to set.</param>
        public new void SetResult<T>(int index, T value)
        {
            base.SetResult(index, value);

            // Get name from index
            string name = GetResultNameByIndex(index);
            NotifyResultChanged(name, value);
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
            // TODO: Implement mapping from index -> result name
            throw new NotImplementedException();
        }
    }
}
