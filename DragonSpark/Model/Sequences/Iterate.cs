using System.Collections.Generic;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Query.Construction;

namespace DragonSpark.Model.Sequences
{
	public sealed class Iterate<T> : IIterate<T>
	{
		public static Iterate<T> Default { get; } = new Iterate<T>();

		Iterate() : this(Enter<T>.Default, CollectionSelection<T>.Default, Enumerate<T>.Default) {}

		readonly ISelect<ICollection<T>, Store<T>> _collection;

		readonly IEnter<T>     _enter;
		readonly IEnumerate<T> _enumerate;

		public Iterate(IEnter<T> enter, ISelect<ICollection<T>, Store<T>> collection, IEnumerate<T> enumerate)
		{
			_enter      = enter;
			_collection = collection;
			_enumerate  = enumerate;
		}

		public Store<T> Get(IEnumerable<T> parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return _enter.Get(array);
				case ICollection<T> collection:
					return _collection.Get(collection);
				default:
					return _enumerate.Get(parameter.GetEnumerator());
			}
		}
	}
}