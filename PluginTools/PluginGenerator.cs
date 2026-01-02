/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     PluginTools
 * FILE:        PluginGenerator.cs
 * PURPOSE:     Generates C# plugin classes dynamically from symbol and method specifications using names instead of indices, with JSON serialization support.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

using System.Text;
using System.Text.Json;
using Plugins.Enums;

namespace PluginTools
{
    /// <summary>
    /// Generate a C# plugin class from symbol and method specifications.
    /// </summary>
    public class PluginGenerator
    {
        /// <summary>
        /// The plugin name
        /// </summary>
        private readonly string _pluginName;

        /// <summary>
        /// The version
        /// </summary>
        private readonly string _version;

        /// <summary>
        /// The description
        /// </summary>
        private readonly string _description;

        /// <summary>
        /// Symbols keyed by name
        /// </summary>
        private readonly Dictionary<string, SymbolSpec> _symbols = new();

        /// <summary>
        /// Methods keyed by CommandId
        /// </summary>
        private readonly Dictionary<int, MethodSpec> _methods = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginGenerator"/> class.
        /// </summary>
        /// <param name="pluginName">Name of the plugin.</param>
        /// <param name="version">The version.</param>
        /// <param name="description">The description.</param>
        public PluginGenerator(string pluginName, string version, string description)
        {
            _pluginName = pluginName;
            _version = version;
            _description = description;
        }

        /// <summary>
        /// Adds the symbol.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="direction">The direction.</param>
        public void AddSymbol(string name, Type type, DirectionType direction)
            => _symbols[name] = new SymbolSpec(name, type, direction);

        /// <summary>
        /// Removes the symbol.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveSymbol(string name)
            => _symbols.Remove(name);

        /// <summary>
        /// Adds the method.
        /// </summary>
        /// <param name="method">The method.</param>
        public void AddMethod(MethodSpec method)
        {
            // Ensure all referenced symbols exist
            foreach (var name in method.InputNames.Append(method.OutputName))
            {
                if (!_symbols.ContainsKey(name))
                    throw new ArgumentException($"Symbol '{name}' does not exist. Add it before adding the method.");
            }

            if (_methods.ContainsKey(method.CommandId))
                throw new ArgumentException($"CommandId '{method.CommandId}' already exists.");

            _methods[method.CommandId] = method;
        }

        /// <summary>
        /// Removes the method.
        /// </summary>
        /// <param name="commandId">The command identifier.</param>
        public void RemoveMethod(int commandId)
            => _methods.Remove(commandId);

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            // Build a lookup table for symbol names -> indices
            var symbolsArray = _symbols.Values.ToArray();
            var nameToIndex = symbolsArray
                .Select((s, i) => new { s.Name, Index = i })
                .ToDictionary(x => x.Name, x => x.Index);

            var sb = new StringBuilder();

            sb.AppendLine("using Plugins;");
            sb.AppendLine("using Plugins.Enums;");
            sb.AppendLine("using Plugins.Interfaces;");
            sb.AppendLine();
            sb.AppendLine("namespace GeneratedPlugins");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {_pluginName} : IPlugin, ISymbolProvider");
            sb.AppendLine("    {");
            sb.AppendLine("        private IPluginContext _context;");
            sb.AppendLine($"        public string Name => \"{_pluginName}\";");
            sb.AppendLine($"        public string Version => \"{_version}\";");
            sb.AppendLine($"        public string Description => \"{_description}\";");
            sb.AppendLine();

            // Symbols
            sb.AppendLine("        public IReadOnlyList<SymbolDefinition> GetSymbols() => new List<SymbolDefinition>");
            sb.AppendLine("        {");
            foreach (var s in symbolsArray)
            {
                sb.AppendLine($"            new SymbolDefinition(\"{s.Name}\", SymbolType.Data, typeof({s.Type.Name}), DirectionType.{s.Direction}),");
            }
            sb.AppendLine("        };");
            sb.AppendLine();

            // Initialize
            sb.AppendLine("        public void Initialize(IPluginContext context) => _context = context;");
            sb.AppendLine();

            // Execute
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
            foreach (var m in _methods.Values)
            {
                sb.AppendLine($"                case {m.CommandId}:");
                sb.AppendLine("                {");

                var inputs = string.Join(", ", m.InputNames.Select(n => $"context.GetVariable<{_symbols[n].Type.Name}>({nameToIndex[n]})"));
                var outputIndex = nameToIndex[m.OutputName];
                sb.AppendLine($"                    context.SetResult({outputIndex}, {m.OperationCode("a", "b")});");
                sb.AppendLine("                    break;");
                sb.AppendLine("                }");
            }
            sb.AppendLine("                default: throw new ArgumentOutOfRangeException();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            // Unmanaged execution
            sb.AppendLine("        private static void ExecuteUnmanaged(int id, IUnmanagedPluginContext context)");
            sb.AppendLine("        {");
            sb.AppendLine("            switch(id)");
            sb.AppendLine("            {");
            foreach (var m in _methods.Values)
            {
                sb.AppendLine($"                case {m.CommandId}:");
                sb.AppendLine("                {");

                var inputs = string.Join(", ", m.InputNames.Select(n => $"context.GetVariable<{_symbols[n].Type.Name}>({nameToIndex[n]})"));
                var outputIndex = nameToIndex[m.OutputName];
                sb.AppendLine($"                    context.SetResult({outputIndex}, {m.OperationCode("a", "b")});");
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

        /// <summary>
        /// Exports the definition.
        /// </summary>
        /// <returns></returns>
        public PluginDefinition ExportDefinition()
        {
            return new PluginDefinition
            {
                PluginName = _pluginName,
                Version = _version,
                Description = _description,
                Symbols = _symbols.Values.ToList(),
                Methods = _methods.Values.ToList()
            };
        }

        /// <summary>
        /// Loads the definition.
        /// </summary>
        /// <param name="def">The definition.</param>
        public void LoadDefinition(PluginDefinition def)
        {
            _symbols.Clear();
            foreach (var s in def.Symbols)
                _symbols[s.Name] = s;

            _methods.Clear();
            foreach (var m in def.Methods)
                _methods[m.CommandId] = m;
        }

        /// <summary>
        /// Saves the json.
        /// </summary>
        /// <param name="path">The path.</param>
        public void SaveJson(string path)
        {
            var json = JsonSerializer.Serialize(ExportDefinition(), new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Loads the json.
        /// </summary>
        /// <param name="path">The path.</param>
        public void LoadJson(string path)
        {
            var def = JsonSerializer.Deserialize<PluginDefinition>(File.ReadAllText(path));
            if (def != null)
                LoadDefinition(def);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Plugin: {_pluginName} (v{_version})");
            sb.AppendLine($"Description: {_description}");
            sb.AppendLine();
            sb.AppendLine("Symbols:");
            foreach (var s in _symbols.Values)
                sb.AppendLine($"  - {s.Name}: {s.Type.Name} ({s.Direction})");

            sb.AppendLine();
            sb.AppendLine("Methods:");
            foreach (var m in _methods.Values)
            {
                sb.AppendLine($"  - {m.Name} (ID {m.CommandId})");
                sb.AppendLine($"      Inputs: {string.Join(", ", m.InputNames)}");
                sb.AppendLine($"      Output: {m.OutputName}");
                sb.AppendLine($"      Operation: {m.OperationCode("a", "b")}");
            }

            return sb.ToString();
        }
    }
}
