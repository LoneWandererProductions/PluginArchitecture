/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Interfaces
 * FILE:        ISymbolProvider.cs
 * PURPOSE:     Optional Interface, to provide symbol definitions for variables and results.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Interfaces
{
    /// <summary>
    /// Optional Interface, to provide symbol definitions for variables and results.
    /// </summary>
    public interface ISymbolProvider
    {
        /// <summary>
        /// Gets the symbols.
        /// </summary>
        /// <returns>a List of Symbol Definitions so an outsider can use the Plugin provided.</returns>
        IReadOnlyList<SymbolDefinition> GetSymbols();
    }
}
