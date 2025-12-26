/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Prototype
 * FILE:        Symbol.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Prototype.Enums;

namespace Prototype
{
    public readonly record struct Symbols(string Name, SymbolType Type, Type DataType);

}
