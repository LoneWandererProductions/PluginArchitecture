/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTests
 * FILE:        AdderPluginManagedTests.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using PrototypSample;

namespace PluginTests
{
    [TestClass]
    public class AdderPluginManagedTests
    {
        [TestMethod]
        public void Adder_Sum_Works_With_Managed_Context()
        {
            // Arrange
            var plugin = new AdderPlugin();
            var context = new ManagedPluginContext(plugin.GetSymbols());

            context.SetVariable(0, 3);
            context.SetVariable(1, 5);

            plugin.Initialize(context);

            // Act
            plugin.Execute(0); // Sum

            // Assert
            int result = context.GetResult<int>(0);
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void Adder_Multiply_Works_With_Managed_Context()
        {
            // Arrange
            var plugin = new AdderPlugin();
            var context = new ManagedPluginContext(plugin.GetSymbols());

            context.SetVariable(0, 4);
            context.SetVariable(1, 6);

            plugin.Initialize(context);

            // Act
            plugin.Execute(1); // Multiply

            // Assert
            int result = context.GetResult<int>(0);
            Assert.AreEqual(24, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Execute_Unknown_Command_Throws()
        {
            var plugin = new AdderPlugin();
            var context = new ManagedPluginContext(plugin.GetSymbols());

            plugin.Initialize(context);
            plugin.Execute(99);
        }
    }
}