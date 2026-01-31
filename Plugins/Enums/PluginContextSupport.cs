/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Enums
 * FILE:        PluginContextSupport.cs
 * PURPOSE:     Supported plugin context types.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Enums
{
    /// <summary>
    /// Signifies supported plugin context types.
    /// </summary>
    [Flags]
    public enum PluginContextSupport
    {
        /// <summary>
        /// None Supported
        /// </summary>
        None = 0,

        /// <summary>
        /// Managed Supported
        /// </summary>
        Managed = 1,


        /// <summary>
        ///  Managed Communication Supported
        /// </summary>
        ManagedCom = 2,

        /// <summary>
        /// Unmanaged Supported
        /// </summary>
        Unmanaged = 4
    }
}