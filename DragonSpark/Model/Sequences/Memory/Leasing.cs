using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory;

[method: MustDisposeResource]
public readonly struct Leasing<T>(Lease<T> owner, Memory<T> reference, uint length) : IDisposable
{
    public static implicit operator Memory<T>(Leasing<T> instance) => instance.AsMemory();

    public static Leasing<T> Default { get; } = new(Lease.Empty<T>(), Memory<T>.Empty, 0);

    [MustDisposeResource]
    public Leasing(Lease<T> owner) : this(owner, (uint)owner.Memory.Length) { }

    [MustDisposeResource]
    public Leasing(Lease<T> owner, uint length) : this(owner, owner.Memory, length) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining), MustDisposeResource]
    public Leasing<T> Size(int size) => new(owner, reference, (uint)size);

    [MethodImpl(MethodImplOptions.AggressiveInlining), MustDisposeResource]
    public Leasing<T> Size(uint size) => new(owner, reference, size);

    public Memory<T> Remaining => reference[(int)Length..];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> AsMemory() => reference[..(int)Length];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan() => reference.Span[..(int)Length];

    public uint Length { get; } = length;

    public uint ActualLength => (uint)reference.Length;

    public T[] Store => owner.Rented;

    public T[] ToArray()
    {
        var result = AsSpan().ToArray();
        Dispose();
        return result;
    }

    public void Dispose()
    {
        var owner1 = owner;
        owner1.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Lease<T>.Enumerator GetEnumerator() => owner.GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArrayExtensions.ArraySegmentValueEnumerable<T> AsValueEnumerable() => owner.AsValueEnumerable();

    [MethodImpl(MethodImplOptions.AggressiveInlining), MustDisposeResource(false)]
    public Lease<T> AsEnumerable() => owner;
}
