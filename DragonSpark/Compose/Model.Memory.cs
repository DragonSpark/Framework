using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DragonSpark.Compose.Model.Memory;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
    [MustDisposeResource]
    public static Leasing<T> Distinct<T>(this Leasing<T> @this)
        => DragonSpark.Model.Sequences.Memory.Distinct<T>.Default.Get(@this);

    [MustDisposeResource]
    public static Leasing<T> Then<T>(this ICollection<T> @this) => CollectionLease<T>.Default.Get(@this);

    public static Span<T> AsSpan<T>(this List<T> @this) => CollectionsMarshal.AsSpan(@this);

    [MustDisposeResource(false)]
    public static Leasing<T> Then<T>(this EnumerableExtensions.ValueEnumerable<T> @this)
        => EnumerableLease<T>.Default.Get(@this);

    [MustDisposeResource]
    public static Leasing<T> Then<T>(this ArrayExtensions.SpanValueEnumerable<T> @this)
        => SpanLeases<T>.Default.Get(@this);

    [MustDisposeResource(false)]
    public static Leasing<T> Then<T>(this ArrayExtensions.ArraySegmentValueEnumerable<T> @this)
        => EnumerableListLease<T>.Default.Get(@this);

    [MustDisposeResource]
    public static Leasing<T> Then<T>(this Lease<T> @this) => new(@this);

    public static LeaseComposer<T> Then<T>(this Leasing<T> @this) => new(@this);

    public static MemoryComposer<T> Then<T>(this Memory<T> @this) => new(@this);

    [MustDisposeResource]
    public static Leasing<T> AsLease<T>(this T @this)
    {
        var result = NewLeasing<T>.Default.Get(1);
        result.AsSpan()[0] = @this;
        return result;
    }
}
