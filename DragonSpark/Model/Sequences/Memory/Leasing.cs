using DragonSpark.Compose;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Sequences.Memory;

public readonly struct Leasing<T> : IDisposable
{
    public static implicit operator Memory<T>(Leasing<T> instance) => instance.AsMemory();

    public static Leasing<T> Default { get; } = new(Lease.Empty<T>(), Memory<T>.Empty, 0);

    readonly Lease<T>  _owner;
    readonly Memory<T> _reference;


    [MustDisposeResource]
    public Leasing(Lease<T> owner) : this(owner, (uint)owner.Memory.Length) { }

    [MustDisposeResource]
    public Leasing(Lease<T> owner, uint length) : this(owner, owner.Memory, length) { }

    [method: MustDisposeResource]
    public Leasing(Lease<T> owner, Memory<T> reference, uint length)
    {
        _owner        = owner;
        _reference    = reference;
        Length        = length;
        _owner.Length = length.Degrade();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining), MustDisposeResource]
    public Leasing<T> Size(int size) => new(_owner, _reference, (uint)size);

    [MethodImpl(MethodImplOptions.AggressiveInlining), MustDisposeResource]
    public Leasing<T> Size(uint size) => new(_owner, _reference, size);

    public Memory<T> Remaining => _reference[(int)Length..];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> AsMemory() => _reference[..(int)Length];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan() => _reference.Span[..(int)Length];

    public uint Length { get; }

    public uint ActualLength => (uint)_reference.Length;

    public T[] Store => _owner.Rented;

    public T[] ToArray()
    {
        var result = AsSpan().ToArray();
        Dispose();
        return result;
    }

    public void Dispose()
    {
        var owner1 = _owner;
        owner1.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Lease<T>.Enumerator GetEnumerator() => _owner.GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArrayExtensions.ArraySegmentValueEnumerable<T> AsValueEnumerable() => _owner.AsValueEnumerable();

    [MethodImpl(MethodImplOptions.AggressiveInlining), MustDisposeResource(false)]
    public Lease<T> AsEnumerable() => _owner;
}