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
        public DataTemplate? DataTemplate { get; set; }
        public DataTemplate? MethodTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is PluginSymbolViewModel vm)
            {
                return vm.IsMethod ? MethodTemplate : DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}