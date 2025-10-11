using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DragonSpark.Model.Results;

public class Variable<T> : IMutable<T?>
{
	public static implicit operator T?(Variable<T> instance) => instance.Get();

	readonly T?[] _store = new T[1];

	public Variable(T? instance = default) => Execute(instance!);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T? Get()
	{
		// ATTRIBUTION: https://x.com/SergioPedri/status/1228752877604265985
		// https://github.com/CommunityToolkit/dotnet/blob/657c6971a8d42655c648336b781639ed96c2c49f/src/CommunityToolkit.HighPerformance/Extensions/ArrayExtensions.1D.cs#L52
		ref var reference = ref MemoryMarshal.GetArrayDataReference(_store);
		ref var result    = ref Unsafe.Add(ref reference, (nint)0);
		return result;
	}

	public void Execute(T? parameter)
	{
		_store[0] = parameter;
	}
}
