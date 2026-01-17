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
        /// <summary>
        /// The context
        /// </summary>
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
            switch (_context)
            {
                case IUnmanagedPluginContext uctx:
                    ExecuteCommand(id, uctx);
                    break;

                case IManagedPluginContext mctx:
                    ExecuteCommand(id, mctx);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported plugin context type");
            }
        }

        private void ExecuteCommand(int id, IManagedPluginContext context)
        {
            switch (id)
            {
                case 0: // Sum
                    {
                        int a = context.GetVariable<int>(0);
                        int b = context.GetVariable<int>(1);
                        context.SetResult(0, a + b);
                        break;
                    }
                case 1: // Multiply
                    {
                        int a = context.GetVariable<int>(0);
                        int b = context.GetVariable<int>(1);
                        context.SetResult(0, a * b);
                        break;
                    }
                // Add more cases for other "methods"
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), $"Unknown command id: {id}");
            }
        }

        private static void ExecuteCommand(int id, IUnmanagedPluginContext context)
        {
            switch (id)
            {
                case 0: // Sum
                    {
                        int a = context.GetVariable<int>(0);
                        int b = context.GetVariable<int>(1);
                        context.SetResult(0, a + b);
                        break;
                    }
                case 1: // Multiply
                    {
                        int a = context.GetVariable<int>(0);
                        int b = context.GetVariable<int>(1);
                        context.SetResult(0, a * b);
                        break;
                    }
                // Add more cases for other "methods"
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), $"Unknown command id: {id}");
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
