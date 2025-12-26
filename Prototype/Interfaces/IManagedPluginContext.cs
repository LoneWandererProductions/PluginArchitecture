namespace Prototype.Interfaces
{
    public interface IManagedPluginContext
        : IPluginContext
    {
        T GetVariable<T>(int index);
        void SetVariable<T>(int index, T value);
        T GetResult<T>(int index);
        void SetResult<T>(int index, T value);
    }
}
