/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginLoader
 * FILE:        SymbolTemplateSelector.cs
 * PURPOSE:     Used in the PluginLoader Control to select between method and data symbol templates.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using System.Windows;
using System.Windows.Controls;

namespace PluginLoader
{
    /// <summary>
    /// Selects between method and data symbol templates.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.DataTemplateSelector" />
    public sealed class SymbolTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the data template.
        /// </summary>
        /// <value>
        /// The data template.
        /// </value>
        public DataTemplate? DataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the method template.
        /// </summary>
        /// <value>
        /// The method template.
        /// </value>
        public DataTemplate? MethodTemplate { get; set; }

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate" /> based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate" /> or <see langword="null" />. The default value is <see langword="null" />.
        /// </returns>
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is PluginSymbolViewModel vm)
            {
                if (vm.IsMethod)
                    return MethodTemplate ?? base.SelectTemplate(item, container);

                if (vm.IsData)
                    return DataTemplate ?? base.SelectTemplate(item, container);
            }

            return base.SelectTemplate(item, container);
        }
    }
}