/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     Plugins
 * FILE:        PluginSymbolExtensions.cs
 * PURPOSE:     Some helpers to resolve plugin symbols via ISymbolProvider not via IPluginContext.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using Plugins.Enums;
using Plugins.Interfaces;

namespace Plugins
{
    /// <summary>
    /// Extension helpers for resolving plugin symbols.
    /// </summary>
    public static class PluginSymbolExtensions
    {
        /// <summary>
        /// Finds a method id by name.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        /// <returns>Id of method.</returns>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        /// <exception cref="System.ArgumentException">Method name is null or empty. - name</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Method '{name}' not found.</exception>
        public static int FindMethod(this ISymbolProvider provider, string name)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Method name is null or empty.", nameof(name));

            foreach (var symbol in provider.GetSymbols())
            {
                if (symbol.Kind == SymbolType.Method &&
                    string.Equals(symbol.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return symbol.Id;
                }
            }

            throw new KeyNotFoundException($"Method '{name}' not found.");
        }

        /// <summary>
        /// Finds a variable id by name.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Id of Input Variable.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Variable '{name}' not found.</exception>
        public static int FindVariable(this ISymbolProvider provider, string name)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            foreach (var symbol in provider.GetSymbols())
            {
                if (symbol.Kind == SymbolType.Data &&
                    symbol.Direction == DirectionType.Input &&
                    string.Equals(symbol.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return symbol.Id;
                }
            }

            throw new KeyNotFoundException($"Variable '{name}' not found.");
        }

        /// <summary>
        /// Finds a result id by name.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Id of result Variable.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Result '{name}' not found.</exception>
        public static int FindResult(this ISymbolProvider provider, string name)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            foreach (var symbol in provider.GetSymbols())
            {
                if (symbol.Kind == SymbolType.Data &&
                    symbol.Direction == DirectionType.Output &&
                    string.Equals(symbol.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return symbol.Id;
                }
            }

            throw new KeyNotFoundException($"Result '{name}' not found.");
        }

        /// <summary>
        /// Finds the internal.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Id of result Variable.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Result '{name}' not found.</exception>
        public static int FindInternal(this ISymbolProvider provider, string name)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            foreach (var symbol in provider.GetSymbols())
            {
                if (symbol.Kind == SymbolType.Data &&
                    symbol.Direction == DirectionType.Internal &&
                    string.Equals(symbol.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return symbol.Id;
                }
            }

            throw new KeyNotFoundException($"Result '{name}' not found.");
        }
    }
}