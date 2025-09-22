using DragonSpark.Model.Results;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DragonSpark.Model.Sequences;

public static class Array
{
	public static Array<T> Of<T>(params T[] elements) => elements;
}

public readonly struct Array<T> : IResult<ImmutableArray<T>>
{
	public static implicit operator ImmutableArray<T>(Array<T> source) => source.Get();

	public static implicit operator Array<T>(T[] source) => new(source);

	public static implicit operator T[](Array<T> source) => source.Open();

	public static Array<T> Empty { get; } = new(Empty<T>.Array);

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
			// ATTRIBUTION: https://x.com/SergioPedri/status/1228752877604265985
			// https://github.com/CommunityToolkit/dotnet/blob/657c6971a8d42655c648336b781639ed96c2c49f/src/CommunityToolkit.HighPerformance/Extensions/ArrayExtensions.1D.cs#L52
			ref var reference = ref MemoryMarshal.GetArrayDataReference(_reference);
			ref var result    = ref Unsafe.Add(ref reference, (nint)(uint)index);
			return ref result!;
		}
	}

	public ImmutableArray<T> Get() => [.._reference];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T[] Open() => _reference;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlyMemory<T> AsMemory() => _reference.AsMemory();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ArrayExtensions.ArraySegmentValueEnumerable<T> AsValueEnumerable() => _reference.AsValueEnumerable();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public SpanEnumerator<T> GetEnumerator() => AsValueEnumerable().GetEnumerator();
}
