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
* `IPluginCommunicator` — Optional Interface for notifying host of changes


### Plugin Interface

```csharp
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        string Description { get; }
        void Execute(int id);
        PluginContextSupport SupportedContexts { get; }
        Task ExecuteAsync(int id);
        void Initialize(IPluginContext context);
        void Shutdown();
    }
```

* `Execute` works with both context types

---

## Example: AdderPlugin

```csharp
    public class AdderPlugin : IPlugin, ISymbolProvider
    {
        private IPluginContext _context;

        /// <inheritdoc />
        public string Name => "Adder";

        /// <inheritdoc />
        public string Version => "1.0.0";

        /// <inheritdoc />
        public string Description => "Test plugin.";

        /// <inheritdoc />
        public IPluginContext Context => _context;

        /// <inheritdoc />
        public PluginContextSupport SupportedContexts =>
            PluginContextSupport.Managed | PluginContextSupport.Unmanaged;


        private int _aIndex;
        private int _bIndex;
        private int _resultIndex;

        /// <inheritdoc />
        public IReadOnlyList<SymbolDefinition> GetSymbols()
        {
            Trace.WriteLine("AdderPlugin.GetSymbols CALLED");

            var lst = new List<SymbolDefinition>
            {
                // Methods
                new SymbolDefinition("Sum", SymbolType.Method, typeof(void)) { Id = 0 },
                new SymbolDefinition("Multiply", SymbolType.Method, typeof(void)) { Id = 1 },
                // Data
                new SymbolDefinition("A", SymbolType.Data, typeof(int)) { Id = 10, Direction = DirectionType.Input },
                new SymbolDefinition("B", SymbolType.Data, typeof(int)) { Id = 11, Direction = DirectionType.Input },
                new SymbolDefinition("Result", SymbolType.Data, typeof(int)) { Id = 12, Direction = DirectionType.Output }
            };

            return lst;
        }


        /// <inheritdoc />
        public void Initialize(IPluginContext context)
        {
            _context = context;

            if (context is IManagedPluginContext mctx)
            {
                _aIndex = mctx.FindVariable("A");
                _bIndex = mctx.FindVariable("B");
                _resultIndex = mctx.FindResult("Result");
            }
            else if (context is IUnmanagedPluginContext uctx)
            {
                _aIndex = uctx.FindVariable("A");
                _bIndex = uctx.FindVariable("B");
                _resultIndex = uctx.FindResult("Result");
            }
        }

        /// <inheritdoc />
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
                        int a = context.GetVariable<int>(_aIndex);
                        int b = context.GetVariable<int>(_bIndex);
                        context.SetResult(_resultIndex, a + b);

                        break;
                    }
                case 1: // Multiply
                    {
                        int a = context.GetVariable<int>(_aIndex);
                        int b = context.GetVariable<int>(_bIndex);
                        context.SetResult(_resultIndex, a * b);

                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), $"Unknown command id: {id}");
            }
        }

        private void ExecuteCommand(int id, IUnmanagedPluginContext context)
        {
            switch (id)
            {
                case 0: // Sum
                {
                        int a = context.GetVariable<int>(_aIndex);
                        int b = context.GetVariable<int>(_bIndex);
                        context.SetResult(context.FindResult("Result"), a + b);

                        break;
                }
                case 1: // Multiply
                {
                        int a = context.GetVariable<int>(_aIndex);
                        int b = context.GetVariable<int>(_bIndex);
                        context.SetResult(context.FindResult("Result"), a * b);

                        break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), $"Unknown command id: {id}");
            }
        }

        /// <inheritdoc />
        public Task ExecuteAsync(int id)
        {
            Execute(id); // simple synchronous execution
            return Task.CompletedTask;
        }

        public void Initialize()
        {
            /* optional */
        }

        /// <inheritdoc />
        public void Shutdown()
        {
            /* optional */
        }
    }
```

---

* Unmanaged context enforces value types
* Managed context allows full object support
* EffectiveSize in symbols is automatic
