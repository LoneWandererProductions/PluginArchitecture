/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTests
 * FILE:        StatefulNetworkPluginTests.cs
 * PURPOSE:     Our stateful plugin tests - Validates that the StatefulNetworkPlugin correctly transitions through states (Disconnected, Connecting, Connected) based on input commands and server status, ensuring proper state management and command handling.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using PrototypSample;

namespace PluginTests
{
    [TestClass]
    public class StatefulNetworkPluginTests
    {
        /// <summary>
        /// Networks the plugin connects successfully when server is awake.
        /// </summary>
        [TestMethod]
        public void NetworkPlugin_ConnectsSuccessfully_WhenServerIsAwake()
        {
            // Arrange
            var plugin = new StatefulNetworkPlugin();

            // MAGIC HERE: The Context builds itself dynamically based on the Plugin's Symbol Requirements!
            var context = new ManagedPluginContextCom(plugin.GetSymbols());

            // Initialize hands the allocated memory back to the plugin
            plugin.Initialize(context);

            // Setup variables using the dynamically mapped memory
            context.SetVariable(context.FindVariable("IsServerAwake"), 1);
            context.SetVariable(context.FindVariable("ConnectionRetries"), 3);

            // Act: Send command 1 (Connect)
            plugin.Execute(1);

            // Evaluate again (moves from Connecting to Connected)
            // Act 2
            plugin.Execute(0);

            // Assert: Use FindResult and GetResult!
            int stateIndex = context.FindResult("CurrentState");
            Assert.AreEqual(2, context.GetResult<int>(stateIndex), "State should be 2 (Connected)");
        }
    }
}