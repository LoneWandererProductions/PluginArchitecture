/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        MethodSpec.cs
 * PURPOSE:     Method specification record for plugins
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace PluginTools
{
    /// <summary>
    /// All needed data for methods to function in plugins.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;PluginTools.MethodSpec&gt;" />
    public record MethodSpec(
        string Name,
        int CommandId,
        string[] InputNames,
        string OutputName,
        Func<string, string, string> OperationCode
    );
}
