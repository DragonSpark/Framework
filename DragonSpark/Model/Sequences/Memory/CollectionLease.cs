using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Memory;

sealed class CollectionLease<T> : ILease<ICollection<T>, T>
{
	public static CollectionLease<T> Default { get; } = new CollectionLease<T>();

	CollectionLease() : this(ListLease<T>.Default, GenericCollectionLease<T>.Default) {}

	readonly ILease<List<T>, T>        _list;
	readonly ILease<ICollection<T>, T> _other;

	public CollectionLease(ILease<List<T>, T> list, ILease<ICollection<T>, T> other)
	{
		_list  = list;
		_other = other;
	}

	public Leasing<T> Get(ICollection<T> parameter)
		=> parameter is List<T> list ? _list.Get(list) : _other.Get(parameter);
}