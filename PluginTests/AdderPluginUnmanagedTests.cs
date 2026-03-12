/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PrototypSample
 * FILE:        AdderPluginUnmanagedTests.cs
 * PURPOSE:     Unmanaged plugin context tests for AdderPlugin
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using PrototypSample;

namespace PluginTests
{
    /// <summary>
    /// Unmanaged plugin context tests for AdderPlugin. These tests ensure that the AdderPlugin can be correctly initialized and executed using an unmanaged plugin context, and that it handles type mismatches appropriately.
    /// </summary>
    [TestClass]
    public class AdderPluginUnmanagedTests
    {
        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>Get a new Context</returns>
        private static UnmanagedPluginContext CreateContext(AdderPlugin plugin)
        {
            var symbols = plugin.GetSymbols();
            return new UnmanagedPluginContext(symbols);
        }

        /// <summary>
        /// Adders the sum works with unmanaged context.
        /// </summary>
        [TestMethod]
        public void Adder_Sum_Works_With_Unmanaged_Context()
        {
            // Arrange
            var plugin = new AdderPlugin();
            var context = CreateContext(plugin);

            context.SetVariable(0, 7); // A
            context.SetVariable(1, 9); // B

            plugin.Initialize(context);

            // Act
            plugin.Execute(0); // Sum

            // Assert
            var result = context.GetResult<int>(0);
            Assert.AreEqual(16, result);
        }

        /// <summary>
        /// Adders the multiply works with unmanaged context.
        /// </summary>
        [TestMethod]
        public void Adder_Multiply_Works_With_Unmanaged_Context()
        {
            // Arrange
            var plugin = new AdderPlugin();
            var context = CreateContext(plugin);

            context.SetVariable(0, 3);
            context.SetVariable(1, 11);

            plugin.Initialize(context);

            // Act
            plugin.Execute(1); // Multiply

            // Assert
            var result = context.GetResult<int>(0);
            Assert.AreEqual(33, result);
        }

        /// <summary>
        /// Unmanaged type of the context rejects wrong type input.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Unmanaged_Context_Rejects_Wrong_Type()
        {
            var plugin = new AdderPlugin();
            var context = CreateContext(plugin);

            context.SetVariable(0, 1.5f); // declared int → boom
        }
    }
}