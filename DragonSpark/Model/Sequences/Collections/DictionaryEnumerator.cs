using System;
using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections
{
	public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IDisposable where TKey : notnull
	{
		readonly IEnumerator<KeyValuePair<TKey, TValue>> _impl;

		public DictionaryEnumerator(IDictionary<TKey, TValue> value) => _impl = value.GetEnumerator();

		public void Reset()
		{
			_impl.Reset();
		}

		public bool MoveNext() => _impl.MoveNext();

		public DictionaryEntry Entry
			=> new DictionaryEntry(_impl.Current.Key ?? throw new InvalidOperationException(), _impl.Current.Value);

		public object Key => _impl.Current.Key;
		public object? Value => _impl.Current.Value;
		public object Current => Entry;

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_impl.Dispose();
		}
	}
}