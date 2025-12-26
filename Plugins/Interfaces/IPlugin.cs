/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Interfaces
 * FILE:        IPlugin.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Interfaces
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }

        string Description { get; }

        void Execute(IPluginContext context);

        Task ExecuteAsync(IPluginContext context);

        void Initialize();
        void Shutdown();
    }

}
