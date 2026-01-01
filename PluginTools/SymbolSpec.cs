/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        SymbolSpec.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;

namespace PluginTools
{
    public record SymbolSpec(string Name, Type Type, DirectionType Direction);
}
