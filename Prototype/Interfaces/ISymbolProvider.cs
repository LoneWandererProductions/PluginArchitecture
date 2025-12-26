/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Prototype.Interfaces
 * FILE:        ISymbolProvider.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Prototype.Interfaces
{
    public interface ISymbolProvider
    {
        IReadOnlyList<SymbolDefinition> GetSymbols();
    }
}
