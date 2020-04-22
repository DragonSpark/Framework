﻿using System.Collections.Generic;
using System.Collections.Specialized;

namespace DragonSpark.Model.Sequences.Collections
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/mattmc3/dotmore
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IOrderedDictionary
	{
#pragma warning disable 109
		new TValue this[int index] { get; set; }

		new TValue this[TKey key] { get; set; }
#pragma warning restore 109
		// TODO ^
		new int Count { get; }
		new ICollection<TKey> Keys { get; }
		new ICollection<TValue> Values { get; }

		new void Add(TKey key, TValue value);

		new void Clear();

		void Insert(int index, TKey key, TValue value);

		int IndexOf(TKey key);

		bool ContainsValue(TValue value);

		bool ContainsValue(TValue value, IEqualityComparer<TValue> comparer);

		new bool ContainsKey(TKey key);

		new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

		new bool Remove(TKey key);

		new void RemoveAt(int index);

		new bool TryGetValue(TKey key, out TValue value);

		TValue GetValue(TKey key);

		void SetValue(TKey key, TValue value);

		KeyValuePair<TKey, TValue> GetItem(int index);

		void SetItem(int index, TValue value);
	}
}