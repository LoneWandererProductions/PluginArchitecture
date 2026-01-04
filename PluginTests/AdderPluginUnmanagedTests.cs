/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTests
 * FILE:        AdderPluginUnmanagedTests.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using PrototypSample;

namespace PluginTests
{
    [TestClass]
    public class AdderPluginUnmanagedTests
    {
        private static UnmanagedPluginContext CreateContext(AdderPlugin plugin)
        {
            var symbols = plugin.GetSymbols();
            return new UnmanagedPluginContext(symbols);
        }

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
            int result = context.GetResult<int>(0);
            Assert.AreEqual(16, result);
        }

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
            int result = context.GetResult<int>(0);
            Assert.AreEqual(33, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Unmanaged_Context_Rejects_Wrong_Type()
        {
            var plugin = new AdderPlugin();
            var context = CreateContext(plugin);

            context.SetVariable<float>(0, 1.5f); // declared int → boom
        }
    }
}