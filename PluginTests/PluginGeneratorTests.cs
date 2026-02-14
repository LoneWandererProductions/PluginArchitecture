/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTests
 * FILE:        PluginGeneratorTests.cs
 * PURPOSE:     Tests our Code Generator for plugins, ensuring it produces correct code based on the MethodSpec and symbol definitions.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugins.Enums;
using PluginTools;

namespace PluginTests
{
    [TestClass]
    public class PluginGeneratorTests
    {
        /// <summary>
        /// Generates the with valid binary method produces correct indices and logic.
        /// </summary>
        [TestMethod]
        public void Generate_WithValidBinaryMethod_ProducesCorrectIndicesAndLogic()
        {
            // Arrange
            var generator = new PluginGenerator("MathPlugin", "1.0.0", "Unit Test Plugin");

            // 1. Add Symbols (Order matters for index verification)
            generator.AddSymbol("VarA", typeof(int), DirectionType.Input);    // Index 0
            generator.AddSymbol("VarB", typeof(int), DirectionType.Input);    // Index 1
            generator.AddSymbol("VarOut", typeof(int), DirectionType.Output); // Index 2

            // 2. Add Method using the MethodSpec record
            var method = new MethodSpec(
                Name: "Multiply",
                CommandId: 50,
                InputNames: new[] { "VarA", "VarB" },
                OutputName: "VarOut",
                OperationCode: (a, b) => $"{a} * {b}"
            );
            generator.AddMethod(method);

            // Act
            string code = generator.Generate();

            // Assert
            // Check if context calls use the correct indices for the variables
            // VarA should be GetVariable<Int32>(0)
            // VarB should be GetVariable<Int32>(1)
            // VarOut should be SetResult(2, ...)

            StringAssert.Contains(code, "context.GetVariable<Int32>(0)");
            StringAssert.Contains(code, "context.GetVariable<Int32>(1)");
            StringAssert.Contains(code, "context.SetResult(2");

            // Verify the operation logic is correctly wrapped
            StringAssert.Contains(code, "context.GetVariable<Int32>(0) * context.GetVariable<Int32>(1)");
        }

        /// <summary>
        /// Generates the with insufficient inputs throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Generate_WithInsufficientInputs_ThrowsException()
        {
            // Arrange
            var generator = new PluginGenerator("FailPlugin", "1.0.0", "Test");
            generator.AddSymbol("In1", typeof(int), DirectionType.Input);
            generator.AddSymbol("Out1", typeof(int), DirectionType.Output);

            // Creating a spec with only ONE input name, but the Func expects logic for two
            var method = new MethodSpec("Fail", 1, new[] { "In1" }, "Out1", (a, b) => a);
            generator.AddMethod(method);

            // Act
            generator.Generate(); // This should throw because inputAccessors[1] doesn't exist
        }
    }
}