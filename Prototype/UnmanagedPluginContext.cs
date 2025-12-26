using Prototype.Interfaces;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Prototype
{
    public sealed class UnmanagedPluginContext : IPluginContext
    {
        private readonly byte[] _memory;
        private readonly MemoryLayout _layout;

        public UnmanagedPluginContext(MemoryLayout layout)
        {
            _layout = layout ?? throw new ArgumentNullException(nameof(layout));
            _memory = new byte[_layout.TotalSize];
        }

        public int VariableCount => _layout.Variables.Count;
        public int ResultCount => _layout.Results.Count;

        public T GetVariable<T>(int index) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateVariableAccess<T>(index);
            return Read<T>(_layout.VariableOffsets[index], symbol.EffectiveSize);
        }

        public void SetVariable<T>(int index, T value) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateVariableAccess<T>(index);
            Write(_layout.VariableOffsets[index], symbol.EffectiveSize, value);
        }

        public T GetResult<T>(int index) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateResultAccess<T>(index);
            return Read<T>(_layout.ResultOffsets[index], symbol.EffectiveSize);
        }

        public void SetResult<T>(int index, T value) where T : unmanaged
        {
            EnforceUnmanaged<T>();
            var symbol = ValidateResultAccess<T>(index);
            Write(_layout.ResultOffsets[index], symbol.EffectiveSize, value);
        }

        // ---- internal helpers ----

        private T Read<T>(int offset, int size) where T : unmanaged
        {
            var span = _memory.AsSpan(offset, size);
            return MemoryMarshal.Read<T>(span);
        }

        private void Write<T>(int offset, int size, T value) where T : unmanaged
        {
            var span = _memory.AsSpan(offset, size);
            MemoryMarshal.Write(span, ref value);
        }

        private SymbolDefinition ValidateVariableAccess<T>(int index)
        {
            if (index < 0 || index >= VariableCount)
                throw new IndexOutOfRangeException();

            var symbol = _layout.Variables[index];
            ValidateType<T>(symbol.Type, symbol.EffectiveSize);
            return symbol;
        }

        private SymbolDefinition ValidateResultAccess<T>(int index)
        {
            if (index < 0 || index >= ResultCount)
                throw new IndexOutOfRangeException();

            var symbol = _layout.Results[index];
            ValidateType<T>(symbol.Type, symbol.EffectiveSize);
            return symbol;
        }

        private static void ValidateType<T>(Type declared, int size)
        {
            if (declared != typeof(T))
                throw new InvalidOperationException(
                    $"Type mismatch. Declared: {declared}, Requested: {typeof(T)}");

            if (Unsafe.SizeOf<T>() != size)
                throw new InvalidOperationException(
                    $"Size mismatch for {typeof(T)}. Layout: {size}, CLR: {Unsafe.SizeOf<T>()}");
        }

        private static void EnforceUnmanaged<T>()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                throw new InvalidOperationException(
                    $"{typeof(T)} is not supported in UnmanagedPluginContext");
        }
    }
}
