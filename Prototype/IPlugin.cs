namespace Prototype
{
    public interface IPlugin
    {
        string Name { get; }
        Version Version { get; }

        void Initialize();
        void Execute();
        void Shutdown();
    }

}
