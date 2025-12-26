/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Interfaces
 * FILE:        IManagedPluginContext.cs
 * PURPOSE:     Basic interface for managed plugin contexts.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Interfaces
{
    /// <summary>
    /// Basic interface for managed plugin contexts. We trust the Memory Manager to handle the actual memory layout.
    /// </summary>
    /// <seealso cref="Plugins.Interfaces.IPluginContext" />
    public interface IManagedPluginContext
        : IPluginContext
    {
        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <returns>Get Variable at position</returns>
        T GetVariable<T>(int index);

        /// <summary>
        /// Sets the variable.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        void SetVariable<T>(int index, T value);

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <returns>Get Resukt at position</returns>
        T GetResult<T>(int index);

        /// <summary>
        /// Sets the result.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        void SetResult<T>(int index, T value);
    }
}
