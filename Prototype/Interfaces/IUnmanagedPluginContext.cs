namespace Prototype.Interfaces
{
    public interface IUnmanagedPluginContext
        : IPluginContext
    {
        T GetVariable<T>(int index) where T : unmanaged;
        void SetVariable<T>(int index, T value) where T : unmanaged;

        T GetResult<T>(int index) where T : unmanaged;

        void SetResult<T>(int index, T value) where T : unmanaged;
    }

}
