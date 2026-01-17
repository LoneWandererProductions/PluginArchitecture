/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        PluginDefinition.cs
 * PURPOSE:     Generates C# plugin classes dynamically from symbol and method specifications using names instead of indices, with JSON serialization support.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace PluginTools
{
    public class PluginDefinition
    {
        public string PluginName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<SymbolSpec> Symbols { get; set; } = new();
        public List<MethodSpec> Methods { get; set; } = new();
    }
}