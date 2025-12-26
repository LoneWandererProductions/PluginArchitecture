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
    IReadOnlyList<SymbolDefinition> GetSymbols();
    void Execute(IPluginContext context);
    Task ExecuteAsync(IPluginContext context);
    void Initialize();
    void Shutdown();
}
```

* `Execute` works with both context types

---

## Example: AdderPlugin

```csharp
public class AdderPlugin : IPlugin
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
            uctx.SetResult(0, uctx.GetVariable<int>(0) + uctx.GetVariable<int>(1));
        else if (context is IManagedPluginContext mctx)
            mctx.SetResult(0, mctx.GetVariable<int>(0) + mctx.GetVariable<int>(1));
    }

    public Task ExecuteAsync(IPluginContext context)
    {
        Execute(context);
        return Task.CompletedTask;
    }

    public void Initialize() { }
    public void Shutdown() { }
}
```

---

## Usage

```csharp
var layout = new MemoryLayout(variables, results);
var managedContext = new ManagedPluginContext(variableCount, resultCount);
var unmanagedContext = new UnmanagedPluginContext(layout);

plugin.Execute(managedContext);
plugin.Execute(unmanagedContext);
```

* Unmanaged context enforces value types
* Managed context allows full object support
* EffectiveSize in symbols is automatic
