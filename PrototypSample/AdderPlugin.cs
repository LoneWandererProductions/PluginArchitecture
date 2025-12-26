using Prototype;
using Prototype.Enums;
using Prototype.Interfaces;

namespace PrototypSample
{
    public class AdderPlugin // You can implement IPlugin/ISymbolProvider as needed
    {
        public string Name => "Adder";
        public string Version => "1.0.0";

        public IReadOnlyList<SymbolDefinition> GetSymbols() => new List<SymbolDefinition>
        {
            new SymbolDefinition("A", SymbolType.Data, typeof(int)),
            new SymbolDefinition("B", SymbolType.Data, typeof(int)),
            new SymbolDefinition("Result", SymbolType.Data, typeof(int))
        };

        public void Execute(IPluginContext context)
        {
            if (context is IUnmanagedPluginContext uctx)
            {
                int a = uctx.GetVariable<int>(0);
                int b = uctx.GetVariable<int>(1);
                uctx.SetResult(0, a + b);
            }
            else if (context is IManagedPluginContext mctx)
            {
                int a = mctx.GetVariable<int>(0);
                int b = mctx.GetVariable<int>(1);
                mctx.SetResult(0, a + b);
            }
            else
            {
                throw new InvalidOperationException("Unsupported plugin context type");
            }
        }

        public Task ExecuteAsync(IPluginContext context)
        {
            Execute(context); // simple synchronous execution
            return Task.CompletedTask;
        }

        public void Initialize() { /* optional */ }
        public void Shutdown() { /* optional */ }
    }
}
