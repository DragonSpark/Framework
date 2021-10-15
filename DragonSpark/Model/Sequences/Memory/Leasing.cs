using NetFabric.Hyperlinq;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Sequences.Memory;

public readonly struct Leasing<T> : IDisposable
{
	public static implicit operator Memory<T>(Leasing<T> instance) => instance.AsMemory();

	public static Leasing<T> Default { get; } = new(Lease.Empty<T>(), Memory<T>.Empty, 0);

	readonly Lease<T>  _owner;
	readonly Memory<T> _reference;

	public Leasing(Lease<T> owner) : this(owner, (uint)owner.Memory.Length) {}

	public Leasing(Lease<T> owner, uint length) : this(owner, owner.Memory, length) {}

	public Leasing(Lease<T> owner, Memory<T> reference, uint length)
	{
		_owner     = owner;
		_reference = reference;
		Length     = length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Leasing<T> Size(int size) => new(_owner, _reference, (uint)size);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Leasing<T> Size(uint size) => new(_owner, _reference, size);

	public Memory<T> Remaining => _reference[(int)Length..];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Memory<T> AsMemory() => _reference[..(int)Length];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T> AsSpan() => _reference.Span[..(int)Length];

	public uint Length { get; }

	public uint ActualLength => (uint)_reference.Length;

	public T[] ToArray()
	{
		var result = AsSpan().ToArray();
		Dispose();
		return result;
	}

	public void Dispose()
	{
		var owner = _owner;
		owner.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Lease<T>.Enumerator GetEnumerator() => _owner.GetEnumerator();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ArrayExtensions.ArraySegmentValueEnumerable<T> AsValueEnumerable() => _owner.AsValueEnumerable();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Lease<T> AsEnumerable() => _owner;
}