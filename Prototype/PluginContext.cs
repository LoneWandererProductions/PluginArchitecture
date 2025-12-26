/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Prototype
 * FILE:        PluginContext.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Prototype.Interfaces;

namespace Prototype
{
    public sealed class ManagedPluginContext : IManagedPluginContext
    {
        private readonly object[] _variables;
        private readonly object[] _results;

        public int VariableCount => throw new NotImplementedException();

        public int ResultCount => throw new NotImplementedException();

        public T GetVariable<T>(int index) => (T)_variables[index];
        public void SetVariable<T>(int index, T value) => _variables[index] = value;

        public T GetResult<T>(int index) => (T)_results[index];
        public void SetResult<T>(int index, T value) => _results[index] = value;
    }
}
