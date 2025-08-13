using System.Runtime.InteropServices;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Objects;

public interface IMaterialize<out T> : ISelect<Array<byte>, T>;

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