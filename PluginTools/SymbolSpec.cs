/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        SymbolSpec.cs
 * PURPOSE:     Specification record for plugin variables
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;

namespace PluginTools
{
    /// <summary>
    /// Variable specification record for plugins.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;PluginTools.SymbolSpec&gt;" />
    public record SymbolSpec(string Name, Type Type, DirectionType Direction);
}