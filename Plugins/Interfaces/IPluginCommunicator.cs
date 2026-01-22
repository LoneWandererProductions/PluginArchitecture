/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Interfaces
 * FILE:        IPluginCommunicator.cs
 * PURPOSE:     Optional interface for plugins to notify listeners when result values change.
 *              Integrates with the plugin context to provide reactive updates.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Interfaces
{
    /// <summary>
    /// Optional Interface for plugins to notify listeners when result values change.
    /// </summary>
    public interface IPluginCommunicator
    {
        /// <summary>
        /// Occurs when [result changed].
        /// </summary>
        event Action<string, object?> ResultChanged;

        /// <summary>
        /// Notifies the result changed.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void NotifyResultChanged(string name, object? value);
    }

}
