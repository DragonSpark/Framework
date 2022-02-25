using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Results;

public class Variable<T> : IMutable<T?>
{
	readonly T?[] _store = new T[1];

	public Variable(T? instance = default) => Execute(instance!);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T Get()
	{
		// ATTRIBUTION: https://twitter.com/SergioPedri/status/1228752877604265985
		var     view   = Unsafe.As<RawData>(_store);
		ref var item   = ref Unsafe.As<byte, T>(ref view.Data);
		ref var result = ref Unsafe.Add(ref item, 0);
		return result;
	}

	public void Execute(T? parameter)
	{
		_store[0] = parameter;
	}

	sealed class RawData
	{
#pragma warning disable 649
		public IntPtr Length;
#pragma warning restore 649

		public byte Data;
	}
}