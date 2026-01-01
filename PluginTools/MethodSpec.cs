/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        MethodSpec.cs
 * PURPOSE:     Your file purpose here
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
