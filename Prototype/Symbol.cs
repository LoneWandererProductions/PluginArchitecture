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
    public record Symbol(string Name, SymbolType Type, Type DataType);

}
