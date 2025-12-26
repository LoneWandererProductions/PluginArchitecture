namespace Prototype
{
    public class MemoryLayout
    {
        public IReadOnlyList<SymbolDefinition> Variables { get; }
        public IReadOnlyList<SymbolDefinition> Results { get; }

        public int[] VariableOffsets { get; }
        public int[] ResultOffsets { get; }

        public int TotalSize { get; }

        public MemoryLayout(List<SymbolDefinition> variables, List<SymbolDefinition> results)
        {
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
            Results = results ?? throw new ArgumentNullException(nameof(results));

            VariableOffsets = new int[variables.Count];
            ResultOffsets = new int[results.Count];

            int offset = 0;

            for (int i = 0; i < variables.Count; i++)
            {
                VariableOffsets[i] = offset;
                offset += variables[i].EffectiveSize;
            }

            for (int i = 0; i < results.Count; i++)
            {
                ResultOffsets[i] = offset;
                offset += results[i].EffectiveSize;
            }

            TotalSize = offset;
        }
    }
}
