/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Interfaces
 * FILE:        IUnmanagedPluginContext.cs
 * PURPOSE:     Unmanaged plugin context interface. Only allows unmanaged data types.
 *              The Idea is to reduce Memory footprint and increacse performance.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Interfaces
{
    /// <summary>
    /// Unmanaged plugin context interface. Only allows unmanaged data types.
    /// </summary>
    /// <seealso cref="Plugins.Interfaces.IPluginContext" />
    public interface IUnmanagedPluginContext
        : IPluginContext
    {
        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        T GetVariable<T>(int index) where T : unmanaged;

        /// <summary>
        /// Sets the variable.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        void SetVariable<T>(int index, T value) where T : unmanaged;

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        T GetResult<T>(int index) where T : unmanaged;

        /// <summary>
        /// Sets the result.
        /// </summary>
        /// <typeparam name="T">Generic DataType</typeparam>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        void SetResult<T>(int index, T value) where T : unmanaged;
    }

}
