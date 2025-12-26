/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Prototype.Interfaces
 * FILE:        IPlugin.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Prototype.Interfaces
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }

        void Execute(IPluginContext context);

        Task ExecuteAsync(IPluginContext context);

        void Initialize();
        void Shutdown();
    }

}
