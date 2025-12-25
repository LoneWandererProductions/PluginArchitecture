/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PrototypSample
 * FILE:        AdderPlugin.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Prototype;
using Prototype.Enums;
using Prototype.Interfaces;

namespace PrototypSample
{
    public class AdderPlugin : IPlugin, ISymbolProvider
    {
        public string Name => "Adder";
        public string Version => "1.0.0";

        public IReadOnlyList<Symbol> GetSymbols() => new List<Symbol>
    {
        new Symbol("A", SymbolType.Data, typeof(int)),
        new Symbol("B", SymbolType.Data, typeof(int)),
        new Symbol("Result", SymbolType.Data, typeof(int))
    };

        public void Execute(PluginContext context)
        {
            int a = (int)context.GetVariable(0);
            int b = (int)context.GetVariable(1);
            context.SetResult(0, a + b);
        }

        public Task ExecuteAsync(PluginContext context)
        {
            // simple sync plugin; just wrap in Task
            Execute(context);
            return Task.CompletedTask;
        }

        public void Initialize() { /* optional */ }
        public void Shutdown() { /* optional */ }
    }

}
