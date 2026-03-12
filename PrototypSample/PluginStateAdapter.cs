/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PrototypSample
 * FILE:        PluginStateAdapter.cs
 * PURPOSE:     Sample adapter that allows the State Engine to interact with the Plugin's memory layout for resource management and logging.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Core.StateExecutive.Interfaces;
using Plugins.Interfaces;

namespace PrototypSample
{
    /// <inheritdoc />
    /// <summary>
    /// Adapts the Plugin's memory layout so the State Engine can read/write to it.
    /// </summary>
    public class PluginStateAdapter : IStateContext
    {
        /// <summary>
        /// The plugin context
        /// </summary>
        private readonly IManagedPluginContext _pluginContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginStateAdapter"/> class.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        public PluginStateAdapter(IManagedPluginContext pluginContext)
        {
            _pluginContext = pluginContext;
        }

        /// <inheritdoc />
        public bool HasResource(string key, int amount = 1)
        {
            int index = _pluginContext.FindVariable(key);
            if (index == -1) return false;

            // Assuming for this example that resources are stored as integers (e.g., retries, buffer size)
            int currentAmount = _pluginContext.GetVariable<int>(index);
            return currentAmount >= amount;
        }

        /// <inheritdoc />
        public bool TryClaimResource(string key, int amount = 1)
        {
            if (!HasResource(key, amount)) return false;

            int index = _pluginContext.FindVariable(key);
            int currentAmount = _pluginContext.GetVariable<int>(index);

            // Atomically consume the resource in the Plugin's memory
            _pluginContext.SetVariable(index, currentAmount - amount);
            return true;
        }

        /// <inheritdoc />
        public void Log(string message)
        {
            // You could map this to a specific string Result in the plugin,
            // or write to a standard logger.
            Console.WriteLine($"[GPSE Plugin Log]: {message}");
        }
    }
}