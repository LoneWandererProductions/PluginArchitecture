/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Interfaces
 * FILE:        IPluginContext.cs
 * PURPOSE:     Basic plugin context interface.
 *              Contexts defines access to variables and results by index.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// Gets the variable count.
        /// </summary>
        /// <value>
        /// The variable count.
        /// </value>
        int VariableCount { get; }

        /// <summary>
        /// Gets the result count.
        /// </summary>
        /// <value>
        /// The result count.
        /// </value>
        int ResultCount { get; }

        /// <summary>
        /// Finds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Id by Name.</returns>
        int Find(string name);

        /// <summary>
        /// Finds the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>Id by Symbol Definition.</returns>
        int Find(SymbolDefinition symbol);

        /// <summary>
        /// Finds the variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Find Variable by Name.</returns>
        int FindVariable(string name);

        /// <summary>
        /// Finds the result.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Find result variable by Name.</returns>
        int FindResult(string name);
    }
}