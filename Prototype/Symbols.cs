/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Prototype
 * FILE:        Symbols.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Prototype.Enums;
using System.Reflection;

namespace Prototype
{
    public class Symbols
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SymbolType Kind { get; set; }

        public TypeInfo Type { get; set; }

        public DirectionType Direction { get; set; }

        public string? Description { get; set; }

    }
}