/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Enums
 * FILE:        SymbolType.cs
 * PURPOSE:     Mostly needed for Symbol File to define if a symbol is a method or data.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Enums
{
    /// <summary>
    /// The type of a symbol.
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// The method Type
        /// Ignored by UnmanagedPluginContext
        /// </summary>
        Method = 0,

        /// <summary>
        /// The data Type.
        /// DirectionType applies
        /// Input, memory allocated for reading.
        /// Output, memory allocated for writing.
        /// InOut,memory allocated for both.
        /// </summary>
        Data = 1
    }
}