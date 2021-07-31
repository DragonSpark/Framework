using NetFabric.Hyperlinq;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Memory
{
	public static class LeaseExtensions
	{
		public static Lease<T> Distinct<T>(this in Lease<T> @this) => Memory.Distinct<T>.Default.Get(@this);

		public static Lease<T> AsLease<T>(this ICollection<T> @this) => CollectionLease<T>.Default.Get(@this);

		public static Lease<T> AsLease<T>(this EnumerableExtensions.ValueEnumerableWrapper<T> @this)
			=> EnumerableLease<T>.Default.Get(@this);
	}
}