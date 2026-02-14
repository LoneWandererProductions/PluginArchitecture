/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Runtime
 * FILE:        CycleRunner.cs
 * PURPOSE:     Manages the cyclic execution of a list of plugins.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using System.Diagnostics;
using Plugins.Interfaces;

namespace Plugins.Runtime
{
    /// <summary>
    /// Simple Cycle Runner that executes a list of plugin steps in a loop with a specified cycle time.
    /// </summary>
    public class CycleRunner
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly IPluginContext _context;

        /// <summary>
        /// The steps
        /// </summary>
        private readonly List<ExecutionStep> _steps = new();

        /// <summary>
        /// The cancellation token source for stopping the cycle
        /// </summary>
        private CancellationTokenSource? _cts;

        /// <summary>
        /// The cycle task
        /// </summary>
        private Task? _cycleTask;

        /// <summary>
        /// Gets or sets the cycle time ms.
        /// </summary>
        /// <value>
        /// The cycle time ms.
        /// </value>
        public int CycleTimeMs { get; set; } = 10;

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning => _cycleTask is { IsCompleted: false };

        /// <summary>
        /// Initializes a new instance of the <see cref="CycleRunner"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CycleRunner(IPluginContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a plugin execution step to the cycle.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <exception cref="System.InvalidOperationException">Plugin {plugin.Name} does not implement ISymbolProvider</exception>
        public void AddStep(IPlugin plugin, string methodName)
        {
            // Uses our extension method to resolve the ID once during setup
            // This is the "Linker" phase happening before the loop starts.
            if (plugin is ISymbolProvider provider)
            {
                int id = provider.FindMethod(methodName);
                _steps.Add(new ExecutionStep(plugin, id));
            }
            else
            {
                // Fallback or error if plugin doesn't provide symbols
                throw new InvalidOperationException($"Plugin {plugin.Name} does not implement ISymbolProvider");
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (IsRunning) return;

            _cts = new CancellationTokenSource();
            _cycleTask = Task.Run(() => RunLoop(_cts.Token));
        }

        /// <summary>
        /// Stops the asynchronous.
        /// </summary>
        public async Task StopAsync()
        {
            if (_cts == null) return;

            _cts.Cancel();
            if (_cycleTask != null)
            {
                try
                {
                    await _cycleTask;
                }
                catch (OperationCanceledException)
                {
                }
            }

            _cts = null;
            _cycleTask = null;
        }

        /// <summary>
        /// Runs the loop.
        /// </summary>
        /// <param name="token">The token.</param>
        private async Task RunLoop(CancellationToken token)
        {
            var stopwatch = new Stopwatch();

            while (!token.IsCancellationRequested)
            {
                stopwatch.Restart();

                // --- THE CYCLE ---

                // 1. Read Inputs (Optional: Update Context from external world)
                // This is where you would read from hardware, databases, or other sources and update the plugin context accordingly.
                // Right now, we assume the context is updated externally or plugins read directly from it, so we skip this step in the example.

                // 2. Execute Logic
                foreach (var step in _steps)
                {
                    // This is the "Hot Path" - very fast
                    step.Plugin.Execute(step.MethodId);
                }

                // 3. Write Outputs (Optional: Push Context to external world)

                // -----------------

                stopwatch.Stop();

                // Wait for the remainder of the cycle time (PLC behavior)
                int elapsed = (int)stopwatch.ElapsedMilliseconds;
                int wait = CycleTimeMs - elapsed;

                if (wait > 0) await Task.Delay(wait, token);
            }
        }

        /// <summary>
        /// Simple wrapper to avoid closure allocation in the loop
        /// </summary>
        private readonly struct ExecutionStep
        {
            /// <summary>
            /// The plugin
            /// </summary>
            public readonly IPlugin Plugin;

            /// <summary>
            /// The method identifier
            /// </summary>
            public readonly int MethodId;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExecutionStep"/> struct.
            /// </summary>
            /// <param name="plugin">The plugin.</param>
            /// <param name="methodId">The method identifier.</param>
            public ExecutionStep(IPlugin plugin, int methodId)
            {
                Plugin = plugin;
                MethodId = methodId;
            }
        }
    }
}