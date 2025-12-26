# Prototype Plugin System

**Author:** Peter Geinitz (Wayfarer)
**License:** See COPYING in top-level directory

---

## Overview

C# plugin framework supporting **managed** and **unmanaged memory contexts**.

* Managed: full CLR objects (strings, lists, etc.)
* Unmanaged: contiguous memory for value types only

---

## Key Concepts

### Symbols

```csharp
public class SymbolDefinition
{
    public string Name { get; }
    public SymbolType Kind { get; }
    public Type Type { get; }
    public int? Size { get; set; }
    public int EffectiveSize => Size ?? Marshal.SizeOf(Type);
}
```

* Defines plugin variables and results
* `EffectiveSize` computed automatically

### Plugin Contexts

* `IManagedPluginContext` — supports all CLR types
* `IUnmanagedPluginContext` — supports unmanaged types only

### Plugin Interface

```csharp
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        string Description { get; }
        void Execute(int id);
        Task ExecuteAsync(int id);
        void Initialize(IPluginContext context);
        void Shutdown();
    }
```

* `Execute` works with both context types

---

## Example: AdderPlugin

```csharp
    public class AdderPlugin: IPlugin, ISymbolProvider
    {
        private IPluginContext _context;

        public string Name => "Adder";
        public string Version => "1.0.0";

        public string Description => "Test plugin.";

        public IReadOnlyList<SymbolDefinition> GetSymbols() => new List<SymbolDefinition>
        {
            new SymbolDefinition("A", SymbolType.Data, typeof(int)),
            new SymbolDefinition("B", SymbolType.Data, typeof(int)),
            new SymbolDefinition("Result", SymbolType.Data, typeof(int))
        };

        public void Execute(int id)
        {
            if (_context is IUnmanagedPluginContext uctx)
            {
                int a = uctx.GetVariable<int>(0);
                int b = uctx.GetVariable<int>(1);
                uctx.SetResult(0, a + b);
            }
            else if (_context is IManagedPluginContext mctx)
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

        public Task ExecuteAsync(int id)
        {
            Execute(id); // simple synchronous execution
            return Task.CompletedTask;
        }

        public void Initialize() { /* optional */ }
        public void Shutdown() { /* optional */ }

        public void Initialize(IPluginContext context) { _context = context; }
    }
```

---

* Unmanaged context enforces value types
* Managed context allows full object support
* EffectiveSize in symbols is automatic
