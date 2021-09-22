﻿using DragonSpark.Compose.Model.Memory;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static DragonSpark.Model.Sequences.Memory.Lease<T> Distinct<T>(this DragonSpark.Model.Sequences.Memory.Lease<T> @this)
			=> DragonSpark.Model.Sequences.Memory.Distinct<T>.Default.Get(@this);

		public static DragonSpark.Model.Sequences.Memory.Lease<T> AsLease<T>(this ICollection<T> @this) => CollectionLease<T>.Default.Get(@this);

		public static Span<T> AsSpan<T>(this List<T> @this) => CollectionsMarshal.AsSpan(@this);

		public static DragonSpark.Model.Sequences.Memory.Lease<T> AsLease<T>(this EnumerableExtensions.ValueEnumerable<T> @this)
			=> EnumerableLease<T>.Default.Get(@this);
		public static DragonSpark.Model.Sequences.Memory.Lease<T> AsLease<T>(this ArrayExtensions.SpanValueEnumerable<T> @this)
			=> SpanLeases<T>.Default.Get(@this);

		public static DragonSpark.Model.Sequences.Memory.Lease<T> AsLease<T>(this ArrayExtensions.ArraySegmentValueEnumerable<T> @this)
			=> EnumerableListLease<T>.Default.Get(@this);

		public static DragonSpark.Model.Sequences.Memory.Lease<T> AsLease<T>(this IMemoryOwner<T> @this) => new(@this);

		public static ArrayExtensions.ArraySegmentValueEnumerable<T> AsValueEnumerable<T>(this Store<T> @this)
			=> @this.Elements.AsValueEnumerable().Take((int)@this.Length);

		/*public static Store<T> AsStore<T>(this Lease<T> @this) => Stores<T>.Default.Get(@this.AsMemory());*/
		public static Store<T> AsStore<T>(this Memory<T> @this) => Stores<T>.Default.Get(@this);

		public static StoredLease<T> WithStore<T>(this DragonSpark.Model.Sequences.Memory.Lease<T> @this) => StoredLeases<T>.Default.Get(@this);
/**/

		public static LeaseSelector<T> Then<T>(this DragonSpark.Model.Sequences.Memory.Lease<T> @this) => new(@this);
		public static MemorySelector<T> Then<T>(this Memory<T> @this) => new(@this);
	}
}