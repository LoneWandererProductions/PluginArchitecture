using System;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prototype
{
    public class Symbol
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SymbolKind Kind { get; set; }

        public TypeInfo Type { get; set; }

        public TypeDirection Direction { get; set; }

        public string? Description { get; set; }

    }
}