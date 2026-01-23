/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        ResultChangedEventArgs.cs
 * PURPOSE:     Message arguments for result changed events for the IPluginCommunicator interface.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Interfaces;

namespace Plugins
{
    /// <summary>
    /// Provides data for the <see cref="IPluginCommunicator.ResultChanged"/> event.
    /// Contains information about the plugin context, result name, index, value, and value type.
    /// </summary>
    public sealed class ResultChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the managed plugin context that raised the event.
        /// </summary>
        public IManagedPluginContext Context { get; }

        /// <summary>
        /// Gets the name of the result that changed.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the index of the result in the context.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the new value of the result.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of the new value, or <c>null</c> if the value is null.
        /// </summary>
        public Type? ValueType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultChangedEventArgs" /> class.
        /// </summary>
        /// <param name="context">The managed plugin context that raised the event.</param>
        /// <param name="name">The name of the result that changed.</param>
        /// <param name="index">The index of the result in the context.</param>
        /// <param name="value">The new value of the result.</param>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// name
        /// </exception>
        public ResultChangedEventArgs(
            IManagedPluginContext context,
            string name,
            int index,
            object? value)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Index = index;
            Value = value;
            ValueType = value?.GetType();
        }
    }
}
