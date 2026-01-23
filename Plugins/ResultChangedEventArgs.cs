using Plugins.Interfaces;

namespace Plugins
{
    public sealed class ResultChangedEventArgs : EventArgs
    {
        public IManagedPluginContext Context { get; }

        public string Name { get; }

        public int Index { get; }

        public object? Value { get; }

        public Type? ValueType { get; }

        public ResultChangedEventArgs(
            IManagedPluginContext context,
            string name,
            int index,
            object? value)
        {
            Context = context;
            Name = name;
            Index = index;
            Value = value;
            ValueType = value?.GetType();
        }
    }
}
