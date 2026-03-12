/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PrototypSample
 * FILE:        StatefulNetworkPlugin.cs
 * PURPOSE:     A sample stateful plugin that simulates connecting to a server and sending data, demonstrating how to use the GPSE for complex internal logic.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Core.StateExecutive;
using Core.StateExecutive.Builder;
using Plugins;
using Plugins.Enums;
using Plugins.Interfaces;

namespace PrototypSample
{
    /// <inheritdoc />
    /// <summary>
    /// Network Communicator Plugin
    /// </summary>
    /// <seealso cref="IPlugin" />
    /// <seealso cref="ISymbolProvider" />
    public class StatefulNetworkPlugin : IPlugin, ISymbolProvider
    {
        /// <inheritdoc />
        public string Name => "Network Communicator";

        /// <inheritdoc />
        public string Version => "1.0.0";

        /// <inheritdoc />
        public string Description => "A stateful plugin that connects to a server and sends data.";

        /// <inheritdoc />
        public PluginContextSupport SupportedContexts => PluginContextSupport.Managed;

        /// <inheritdoc />
        public IPluginContext Context { get; private set; }

        /// <summary>
        /// The engine
        /// </summary>
        private EngineContext _engine;

        /// <summary>
        /// The blackboard
        /// </summary>
        private PluginStateAdapter _blackboard;

        /// <summary>
        /// Gets the symbols.
        /// </summary>
        /// <returns>Symbol List of plugins.</returns>
        public IReadOnlyList<SymbolDefinition> GetSymbols()
        {
            return new List<SymbolDefinition>
            {
                // Inputs from the Host
                new SymbolDefinition("CurrentCommand", SymbolType.Data, typeof(int))
                    { Direction = DirectionType.Input },
                new SymbolDefinition("IsServerAwake", SymbolType.Data, typeof(int)) { Direction = DirectionType.Input },

                // Internal GPSE Resources
                new SymbolDefinition("ConnectionRetries", SymbolType.Data, typeof(int))
                    { Direction = DirectionType.Internal },

                // Outputs back to the Host
                new SymbolDefinition("CurrentState", SymbolType.Data, typeof(int)) { Direction = DirectionType.Output }
            };
        }

        /// <inheritdoc />
        public void Initialize(IPluginContext context)
        {
            Context = context;
            _blackboard = new PluginStateAdapter((IManagedPluginContext)Context);
            _engine = new EngineContext();

            // GPSE Definition
            var disconnectedState = StateBuilder.Create("Disconnected")
                .TransitionTo("Connecting")
                .When(ctx => GetVariableAsInt("CurrentCommand") == 1)
                .OnTransition(ctx => ctx.Log("Connecting..."))
                .OnTransition(ctx => SetResultAsInt("CurrentState", 1))
                .EndTransition()
                .Build();

            var connectingState = StateBuilder.Create("Connecting")
                .TransitionTo("Connected")
                .When(ctx => GetVariableAsInt("IsServerAwake") == 1)
                .OnTransition(ctx => SetResultAsInt("CurrentState", 2))
                .EndTransition()
                .TransitionTo("Disconnected")
                // The GPSE claims the retry token directly from unmanaged/managed memory!
                .Claim("ConnectionRetries", 1)
                .OnTransition(ctx => SetResultAsInt("CurrentState", 0))
                .EndTransition()
                .Build();

            var connectedState = StateBuilder.Create("Connected").Build();

            _engine.RegisterState(disconnectedState);
            _engine.RegisterState(connectingState);
            _engine.RegisterState(connectedState);
            _engine.SetInitialState("Disconnected");

            // FIX: Initialize outputs using Result API
            SetResultAsInt("CurrentState", 0);
        }

        /// <inheritdoc />
        public void Execute(int id)
        {
            // Store the incoming command ID cleanly using your new extensions
            int cmdIndex = this.FindVariable("CurrentCommand");
            ((IManagedPluginContext)Context).SetVariable(cmdIndex, id);

            // Tell the engine to evaluate its rules based on the new memory state
            _engine.Evaluate(_blackboard);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(int id)
        {
            Execute(id);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Shutdown()
        {
            // Clean up resources if necessary
            _engine = null;
        }

        // --- Helper methods using PluginSymbolExtensions ---

        /// <summary>
        /// Gets the variable as int.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private int GetVariableAsInt(string name)
        {
            // Ask the Context directly! It holds the real memory map IDs.
            int id = Context.FindVariable(name);
            return ((IManagedPluginContext)Context).GetVariable<int>(id);
        }

        /// <summary>
        /// Sets the result as int.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        private void SetResultAsInt(string name, int value)
        {
            // Ask the Context directly!
            int id = Context.FindResult(name);
            ((IManagedPluginContext)Context).SetResult(id, value);
        }
    }
}