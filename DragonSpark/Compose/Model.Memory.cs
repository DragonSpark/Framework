using DragonSpark.Compose.Model.Memory;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
	public static Leasing<T> Distinct<T>(this Leasing<T> @this)
		=> DragonSpark.Model.Sequences.Memory.Distinct<T>.Default.Get(@this);

	public static Leasing<T> Then<T>(this ICollection<T> @this) => CollectionLease<T>.Default.Get(@this);

	public static Span<T> AsSpan<T>(this List<T> @this) => CollectionsMarshal.AsSpan(@this);

	public static Leasing<T> Then<T>(this EnumerableExtensions.ValueEnumerable<T> @this)
		=> EnumerableLease<T>.Default.Get(@this);
	public static Leasing<T> Then<T>(this ArrayExtensions.SpanValueEnumerable<T> @this)
		=> SpanLeases<T>.Default.Get(@this);

	public static Leasing<T> Then<T>(this ArrayExtensions.ArraySegmentValueEnumerable<T> @this)
		=> EnumerableListLease<T>.Default.Get(@this);

	public static Leasing<T> Then<T>(this Lease<T> @this) => new(@this);

	public static LeaseSelector<T> Then<T>(this Leasing<T> @this) => new(@this);

	public static MemorySelector<T> Then<T>(this Memory<T> @this) => new(@this);

	public static Leasing<T> AsLease<T>(this T @this)
	{
		var result = NewLeasing<T>.Default.Get(1);
		result.AsSpan()[0] = @this;
		return result;
	}
}