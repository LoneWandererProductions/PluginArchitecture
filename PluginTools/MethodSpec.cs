/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        MethodSpec.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace PluginTools
{
    public record MethodSpec(
        string Name,
        int CommandId,
        int[] InputIndices,
        int OutputIndex,
        Func<string, string, string> OperationCode // operation as string expression
    );
}
