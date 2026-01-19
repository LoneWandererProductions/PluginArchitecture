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

        /// <inheritdoc />
        public string Name => "Adder";

        /// <inheritdoc />
        public string Version => "1.0.0";

        /// <inheritdoc />
        public string Description => "Test plugin.";

        /// <inheritdoc />
        public IPluginContext Context => _context;

        /// <summary>
        /// a index
        /// </summary>
        private int _aIndex;

        /// <summary>
        /// The b index
        /// </summary>
        private int _bIndex;

        /// <summary>
        /// The result index
        /// </summary>
        private int _resultIndex;

        /// <inheritdoc />
        /// <summary>
        /// Gets the symbols.
        /// </summary>
        /// <returns>
        /// a List of Symbol Definitions so an outsider can use the Plugin provided.
        /// </returns>
        public IReadOnlyList<SymbolDefinition> GetSymbols() => new List<SymbolDefinition>
        {
            // Methods
            new SymbolDefinition("Sum", SymbolType.Method, typeof(void)) { Id = 0 },
            new SymbolDefinition("Multiply", SymbolType.Method, typeof(void)) { Id = 1 },
            // Data
            new SymbolDefinition("A", SymbolType.Data, typeof(int)) { Id = 10, Direction = DirectionType.Input },
            new SymbolDefinition("B", SymbolType.Data, typeof(int)) { Id = 11, Direction = DirectionType.Input },
            new SymbolDefinition("Result", SymbolType.Data, typeof(int)) { Id = 12, Direction = DirectionType.Output },

        };

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


        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">id - Unknown command id: {id}</exception>
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
}