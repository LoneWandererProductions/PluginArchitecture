/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        Symbol.cs
 * PURPOSE:     Contains a collection of information about a symbol.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;

namespace Plugins
{
    /// <summary>
    /// Collection of information about a symbol.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;Plugins.Symbols&gt;" />
    public readonly record struct Symbols(string Name, SymbolType Type, Type DataType);
}
