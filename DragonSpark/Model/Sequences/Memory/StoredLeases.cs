using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class StoredLeases<T> : ISelect<Lease<T>, StoredLease<T>>
	{
		public static StoredLeases<T> Default { get; } = new StoredLeases<T>();

		StoredLeases() : this(Stores<T>.Default) {}

		readonly Stores<T> _stores;

		public StoredLeases(Stores<T> stores) => _stores = stores;

		public StoredLease<T> Get(Lease<T> parameter) => new(parameter, _stores.Get(parameter.AsMemory()));
	}
}