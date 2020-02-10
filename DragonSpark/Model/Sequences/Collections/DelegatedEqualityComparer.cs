using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections
{
	public sealed class DelegatedEqualityComparer<T, TKey> : IEqualityComparer<T>
	{
		readonly Func<T, TKey>           _key;
		readonly IEqualityComparer<TKey> _equals;

		public DelegatedEqualityComparer(Func<T, TKey> key) : this(key, EqualityComparer<TKey>.Default) {}

		public DelegatedEqualityComparer(Func<T, TKey> key, IEqualityComparer<TKey> equals)
		{
			_key    = key;
			_equals = @equals;
		}

		public bool Equals(T x, T y) => _equals.Equals(_key(x), _key(y));

		public int GetHashCode(T obj) => _key(obj).GetHashCode();
	}
}