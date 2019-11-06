using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	sealed class CollectionSelection<T> : ISelect<ICollection<T>, Store<T>>
	{
		public static CollectionSelection<T> Default { get; } = new CollectionSelection<T>();

		CollectionSelection() : this(Allotted<T>.Default) {}

		readonly IStores<T> _storage;

		public CollectionSelection(IStores<T> storage) => _storage = storage;

		public Store<T> Get(ICollection<T> parameter)
		{
			var result = _storage.Get(parameter.Count);
			parameter.CopyTo(result.Instance, 0);
			return result;
		}
	}
}