/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Prototype
 * FILE:        PluginContext.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Prototype
{
    /// <summary>
    /// Here we can add a custom memory manager to handle the allocation of variables and results.
    /// </summary>
    public class PluginContext
    {
        private readonly object[] _variables;
        private readonly object[] _results;

        public PluginContext(int variableCount, int resultCount)
        {
            _variables = new object[variableCount];
            _results = new object[resultCount];
        }

        public object GetVariable(int index) => _variables[index];
        public void SetVariable(int index, object value) => _variables[index] = value;

        public object GetResult(int index) => _results[index];
        public void SetResult(int index, object value) => _results[index] = value;

        public int VariableCount => _variables.Length;
        public int ResultCount => _results.Length;
    }

}
