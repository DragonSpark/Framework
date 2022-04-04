using DragonSpark.Model.Results;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Sequences;

public static class Array
{
	public static Array<T> Of<T>(params T[] elements) => elements;
}

public readonly struct Array<T> : IResult<ImmutableArray<T>>
{
	public static implicit operator ImmutableArray<T>(Array<T> source) => source.Get();

	public static implicit operator Array<T>(T[] source) => new Array<T>(source);

	public static implicit operator T[](Array<T> source) => source.Open();

	public static Array<T> Empty { get; } = new Array<T>(Empty<T>.Array);

	readonly T[] _reference;

	public Array(params T[] elements) : this(elements, (uint)elements.Length) {}

	public Array(T[] reference, uint length)
	{
		_reference = reference;
		Length     = length;
	}

	public uint Length { get; }

	public ref readonly T this[uint index] => ref this[(int)index];

	public ref readonly T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			// ATTRIBUTION: https://twitter.com/SergioPedri/status/1228752877604265985
			var     view   = Unsafe.As<RawData>(_reference);
			ref var item   = ref Unsafe.As<byte, T>(ref view.Data);
			ref var result = ref Unsafe.Add(ref item, index);
			// ReSharper disable once RedundantNullForgivingOperator
			return ref result!;
		}
	}

	public ImmutableArray<T> Get() => ImmutableArray.Create(_reference);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T[] Open() => _reference;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ArrayExtensions.ArraySegmentValueEnumerable<T> AsValueEnumerable() => _reference.AsValueEnumerable();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public SpanEnumerator<T> GetEnumerator() => AsValueEnumerable().GetEnumerator();

	sealed class RawData
	{
#pragma warning disable 649
		public IntPtr Length;
#pragma warning restore 649

		public byte Data;
	}
}