using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class GroupMap<T, TKey> : ISelect<T[], IArrayMap<TKey, T>>
	{
		readonly IEqualityComparer<TKey> _comparer;
		readonly Func<T, TKey>           _key;

		public GroupMap(Func<T, TKey> key) : this(key, EqualityComparer<TKey>.Default) {}

		public GroupMap(Func<T, TKey> key, IEqualityComparer<TKey> comparer)
		{
			_key      = key;
			_comparer = comparer;
		}

		public IArrayMap<TKey, T> Get(T[] parameter)
			=> new ArrayMap<TKey, T>(parameter.Take(parameter.Length)
			                                  .GroupBy(_key, _comparer)
			                                  .ToDictionary(x => x.Key, x => x.ToArray().Result())
			                                  .ToStore());
	}
}