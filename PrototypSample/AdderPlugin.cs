/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PrototypSample
 * FILE:        AdderPlugin.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using Plugins.Enums;
using Plugins.Interfaces;

namespace PrototypSample
{
    public class AdderPlugin : IPlugin, ISymbolProvider
    {
        /// <summary>
        /// The context
        /// </summary>
        private IPluginContext _context;

        public string Name => "Adder";
        public string Version => "1.0.0";

        public string Description => "Test plugin.";

        public IPluginContext Context => _context;

        public IReadOnlyList<SymbolDefinition> GetSymbols() => new List<SymbolDefinition>
        {
            new SymbolDefinition("A", SymbolType.Data, typeof(int)) { Direction = DirectionType.Input },
            new SymbolDefinition("B", SymbolType.Data, typeof(int)) { Direction = DirectionType.Input },
            new SymbolDefinition("Result", SymbolType.Data, typeof(int)) { Direction = DirectionType.Output }
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

        public void Initialize()
        {
            /* optional */
        }

        public void Shutdown()
        {
            /* optional */
        }

        public void Initialize(IPluginContext context)
        {
            _context = context;
        }
    }
}