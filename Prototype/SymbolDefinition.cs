/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Prototype
 * FILE:        Symbols.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Prototype.Enums;
using System.Runtime.InteropServices;

namespace Prototype
{
    public class SymbolDefinition
    {
        private string v;
        private SymbolType data;
        private Type type;

        public SymbolDefinition(string name, SymbolType kind, Type type)
        {
            Name = name;
            Kind = kind;
            Type = type;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public SymbolType Kind { get; set; }

        public Type Type { get; set; }

        public DirectionType Direction { get; set; }

        public string? Description { get; set; }

        // Optional user-defined size (null by default)
        public int? Size { get; set; }

        // Computed property
        public int EffectiveSize => Size ?? Marshal.SizeOf(Type);

    }
}