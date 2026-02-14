/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTests
 * FILE:        AdderPluginManagedComTests.cs
 * PURPOSE:     Tests AdderPlugin with ManagedPluginContextCom and verifies ResultChanged notifications.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins;
using PrototypSample;

namespace PluginTests
{
    [TestClass]
    public class AdderPluginManagedComTests
    {
        [TestMethod]
        public void Adder_Sum_Fires_ResultChanged()
        {
            // Arrange
            var plugin = new AdderPlugin();
            var context = new ManagedPluginContextCom(plugin.GetSymbols());
            plugin.Initialize(context);

            int? notifiedValue = null;
            string? notifiedName = null;

            context.ResultChanged += (sender, e) =>
            {
                notifiedName = e.Name;
                notifiedValue = (int)e.Value!;
            };

            context.SetVariable(0, 2);
            context.SetVariable(1, 3);

            // Act
            plugin.Execute(0); // Sum

            // Assert
            var result = context.GetResult<int>(0);
            Assert.AreEqual(5, result);
            Assert.AreEqual("Result", notifiedName);
            Assert.AreEqual(5, notifiedValue);
        }

        [TestMethod]
        public void Adder_Multiply_Fires_ResultChanged()
        {
            // Arrange
            var plugin = new AdderPlugin();
            var context = new ManagedPluginContextCom(plugin.GetSymbols());
            plugin.Initialize(context);

            int? notifiedValue = null;
            string? notifiedName = null;

            context.ResultChanged += (sender, e) =>
            {
                notifiedName = e.Name;
                notifiedValue = (int)e.Value!;
            };

            context.SetVariable(0, 4);
            context.SetVariable(1, 5);

            // Act
            plugin.Execute(1); // Multiply

            // Assert
            var result = context.GetResult<int>(0);
            Assert.AreEqual(20, result);
            Assert.AreEqual("Result", notifiedName);
            Assert.AreEqual(20, notifiedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Execute_Unknown_Command_Throws_With_Com()
        {
            var plugin = new AdderPlugin();
            var context = new ManagedPluginContextCom(plugin.GetSymbols());
            plugin.Initialize(context);

            plugin.Execute(99);
        }
    }
}