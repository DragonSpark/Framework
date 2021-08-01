using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static Lease<T> Distinct<T>(this in Lease<T> @this)
			=> DragonSpark.Model.Sequences.Memory.Distinct<T>.Default.Get(@this);

		public static Lease<T> AsLease<T>(this ICollection<T> @this) => CollectionLease<T>.Default.Get(@this);

		public static Lease<T> AsLease<T>(this EnumerableExtensions.ValueEnumerableWrapper<T> @this)
			=> EnumerableLease<T>.Default.Get(@this);

		public static Lease<T> AsLease<T>(this ReadOnlyListExtensions.ValueEnumerableWrapper<T> @this)
			=> EnumerableListLease<T>.Default.Get(@this);

		public static Lease<T> AsLease<T>(this IMemoryOwner<T> @this) => new(@this);

		/*public static Store<T> AsStore<T>(this Lease<T> @this) => Stores<T>.Default.Get(@this.AsMemory());*/
		public static Store<T> AsStore<T>(this Memory<T> @this) => Stores<T>.Default.Get(@this);

		public static StoredLease<T> WithStore<T>(this Lease<T> @this) => StoredLeases<T>.Default.Get(@this);
	}
}