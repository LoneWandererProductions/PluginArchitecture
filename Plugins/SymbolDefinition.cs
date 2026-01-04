/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        Symbols.cs
 * PURPOSE:     Definition of a symbol used in plugins.
 *              Can describe Methods or Data.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;
using System.Runtime.InteropServices;

namespace Plugins
{
    /// <summary>
    /// Description of a symbol, Method or Data in the plugin system.
    /// </summary>
    public class SymbolDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolDefinition"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="kind">The kind.</param>
        /// <param name="type">The type.</param>
        public SymbolDefinition(string name, SymbolType kind, Type type)
        {
            Name = name;
            Kind = kind;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        /// <value>
        /// The kind.
        /// </value>
        public SymbolType Kind { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public DirectionType Direction { get; init; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// Optional user-defined size (null by default)
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int? Size { get; set; }

        /// <summary>
        /// Computed property
        /// Gets the size of the effective.
        /// </summary>
        /// <value>
        /// The size of the effective.
        /// </value>
        public int EffectiveSize => Size ?? Marshal.SizeOf(Type);
    }
}