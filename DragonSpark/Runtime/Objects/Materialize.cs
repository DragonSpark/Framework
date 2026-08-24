using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Runtime.InteropServices;

namespace DragonSpark.Runtime.Objects;

public sealed class Materialize<T> : IMaterialize<T>
{
    public static Materialize<T> Default { get; } = new();

    Materialize() {}

    public T Get(Array<byte> parameter)
    {
        var handle = GCHandle.Alloc(parameter.Open(), GCHandleType.Pinned);
        try
        {
            return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject()).Verify();
        }
        finally
        {
            handle.Free();
        }
    }
}