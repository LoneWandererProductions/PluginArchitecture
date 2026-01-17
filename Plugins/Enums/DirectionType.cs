/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins.Enums
 * FILE:        DirectionType.cs
 * PURPOSE:     Dewfine the direction type of a method. If it takes variables as input and if it outputs results or both.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

namespace Plugins.Enums
{
    /// <summary>
    /// Direction type of a method.
    /// </summary>
    public enum DirectionType
    {
        /// <summary>
        /// The input
        /// </summary>
        Input = 0,

        /// <summary>
        /// The output
        /// </summary>
        Output = 1,

        /// <summary>
        /// The in out
        /// </summary>
        InOut = 2
    }
}