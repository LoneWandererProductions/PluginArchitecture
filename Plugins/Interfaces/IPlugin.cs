/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Interfaces
 * FILE:        IPlugin.cs
 * PURPOSE:     Basic Plugin Interface.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Interfaces
{
    /// <summary>
    /// General Plugin Interface.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        string Version { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Executes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Execute(int id);

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task ExecuteAsync(int id);

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void Initialize(IPluginContext context);

        /// <summary>
        /// Shuts down this instance.
        /// </summary>
        void Shutdown();
    }
}