/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        PluginGenerator.cs
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
    public static class PluginGenerator
    {
        public static string GeneratePlugin(
            string pluginName,
            string version,
            string description,
            SymbolSpec[] symbols,
            MethodSpec[] methods)
        {
            var sb = new StringBuilder();

            // File header
            sb.AppendLine("using Plugins;");
            sb.AppendLine("using Plugins.Enums;");
            sb.AppendLine("using Plugins.Interfaces;");
            sb.AppendLine();
            sb.AppendLine($"namespace GeneratedPlugins");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {pluginName} : IPlugin, ISymbolProvider");
            sb.AppendLine("    {");
            sb.AppendLine("        private IPluginContext _context;");
            sb.AppendLine($"        public string Name => \"{pluginName}\";");
            sb.AppendLine($"        public string Version => \"{version}\";");
            sb.AppendLine($"        public string Description => \"{description}\";");
            sb.AppendLine();

            // Symbols
            sb.AppendLine("        public IReadOnlyList<SymbolDefinition> GetSymbols() => new List<SymbolDefinition>");
            sb.AppendLine("        {");
            foreach (var s in symbols)
                sb.AppendLine($"            new SymbolDefinition(\"{s.Name}\", SymbolType.Data, typeof({s.Type.Name})),");
            sb.AppendLine("        };");
            sb.AppendLine();

            // Initialize
            sb.AppendLine("        public void Initialize(IPluginContext context) => _context = context;");
            sb.AppendLine();

            // Execute switch
            sb.AppendLine("        public void Execute(int id)");
            sb.AppendLine("        {");
            sb.AppendLine("            switch (_context)");
            sb.AppendLine("            {");
            sb.AppendLine("                case IManagedPluginContext mctx: ExecuteManaged(id, mctx); break;");
            sb.AppendLine("                case IUnmanagedPluginContext uctx: ExecuteUnmanaged(id, uctx); break;");
            sb.AppendLine("                default: throw new InvalidOperationException(\"Unsupported context\");");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            // Managed execution
            sb.AppendLine("        private void ExecuteManaged(int id, IManagedPluginContext context)");
            sb.AppendLine("        {");
            sb.AppendLine("            switch(id)");
            sb.AppendLine("            {");
            foreach (var m in methods)
            {
                sb.AppendLine($"                case {m.CommandId}:");
                sb.AppendLine("                {");
                var inputs = string.Join(", ", m.InputIndices.Select(i => $"context.GetVariable<{symbols[i].Type.Name}>({i})"));
                sb.AppendLine($"                    context.SetResult({m.OutputIndex}, {m.OperationCode("a", "b")});");
                sb.AppendLine("                    break;");
                sb.AppendLine("                }");
            }
            sb.AppendLine("                default: throw new ArgumentOutOfRangeException();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            // Unmanaged execution (similar)
            sb.AppendLine("        private static void ExecuteUnmanaged(int id, IUnmanagedPluginContext context)");
            sb.AppendLine("        {");
            sb.AppendLine("            switch(id)");
            sb.AppendLine("            {");
            foreach (var m in methods)
            {
                sb.AppendLine($"                case {m.CommandId}:");
                sb.AppendLine("                {");
                var inputs = string.Join(", ", m.InputIndices.Select(i => $"context.GetVariable<{symbols[i].Type.Name}>({i})"));
                sb.AppendLine($"                    context.SetResult({m.OutputIndex}, {m.OperationCode("a", "b")});");
                sb.AppendLine("                    break;");
                sb.AppendLine("                }");
            }
            sb.AppendLine("                default: throw new ArgumentOutOfRangeException();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }

}
