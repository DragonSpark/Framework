using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class ListLease<T> : ILease<List<T>, T>
	{
		public static ListLease<T> Default { get; } = new ListLease<T>();

		ListLease() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public ListLease(ILeases<T> leases) => _leases = leases;

		public Lease<T> Get(List<T> parameter)
		{
			var result = _leases.Get((uint)parameter.Count);
			CollectionsMarshal.AsSpan(parameter).CopyTo(result.AsSpan());
			return result;
		}
	}
}