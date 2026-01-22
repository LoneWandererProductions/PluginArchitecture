/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        PluginContextSupport.cs
 * PURPOSE:     Supported plugin context types.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins
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
        /// Unmanaged Supported
        /// </summary>
        Unmanaged = 2
    }
}
