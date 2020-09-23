﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
// ReSharper disable All

// ReSharper disable ComplexConditionExpression

namespace DragonSpark.Model.Sequences.Collections
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/mattmc3/dotmore
	/// </summary>
	public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue> where TKey : notnull
	{
		DelegatedKeyedCollection<TKey, KeyValuePair<TKey, TValue>>? _delegatedKeyedCollection;

		public OrderedDictionary()
		{
			Initialize();
		}

		public OrderedDictionary(IEqualityComparer<TKey> comparer)
		{
			Initialize(comparer);
		}

		public OrderedDictionary(IOrderedDictionary<TKey, TValue> dictionary)
		{
			Initialize();
			foreach (var pair in dictionary)
			{
				_delegatedKeyedCollection?.Add(pair);
			}
		}

		public OrderedDictionary(IOrderedDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			Initialize(comparer);
			foreach (var pair in dictionary)
			{
				_delegatedKeyedCollection?.Add(pair);
			}
		}

		public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items)
		{
			Initialize();
			foreach (var pair in items)
			{
				_delegatedKeyedCollection?.Add(pair);
			}
		}

		public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> comparer)
		{
			Initialize(comparer);
			foreach (var pair in items)
			{
				_delegatedKeyedCollection?.Add(pair);
			}
		}

		/// <summary>
		/// Gets the key comparer for this dictionary
		/// </summary>
		public IEqualityComparer<TKey>? Comparer { [UsedImplicitly] get; private set; }

		void Initialize(IEqualityComparer<TKey>? comparer = default)
		{
			Comparer = comparer;
			_delegatedKeyedCollection = comparer != null
				                            ? new DelegatedKeyedCollection<TKey, KeyValuePair<TKey, TValue>>(x => x.Key,
				                                                                                             comparer)
				                            : new DelegatedKeyedCollection<TKey, KeyValuePair<TKey, TValue>
				                            >(x => x.Key);
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key associated with the value to get or set.</param>
		public TValue this[TKey key]
		{
			get => GetValue(key);
			set => SetValue(key, value);
		}

		/// <summary>
		/// Gets or sets the value at the specified index.
		/// </summary>
		/// <param name="index">The index of the value to get or set.</param>
		public TValue this[int index]
		{
			get => GetItem(index).Value;
			set => SetItem(index, value);
		}

		/// <summary>
		/// Gets the number of items in the dictionary
		/// </summary>
		public int Count => _delegatedKeyedCollection!.Count;

		/// <summary>
		/// Gets all the keys in the ordered dictionary in their proper order.
		/// </summary>
		public ICollection<TKey> Keys => _delegatedKeyedCollection.Select(x => x.Key).ToList();

		/// <summary>
		/// Gets all the values in the ordered dictionary in their proper order.
		/// </summary>
		public ICollection<TValue> Values => _delegatedKeyedCollection.Select(x => x.Value).ToList();

		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add.  The value can be null for reference types.</param>
		public void Add(TKey key, TValue value)
		{
			_delegatedKeyedCollection?.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		/// <summary>
		/// Removes all keys and values from this object.
		/// </summary>
		public void Clear()
		{
			_delegatedKeyedCollection?.Clear();
		}

		/// <summary>
		/// Inserts a new key-value pair at the index specified.
		/// </summary>
		/// <param name="index">The insertion index.  This value must be between 0 and the count of items in this object.</param>
		/// <param name="key">A unique key for the element to add</param>
		/// <param name="value">The value of the element to add.  Can be null for reference types.</param>
		public void Insert(int index, TKey key, TValue value)
		{
			_delegatedKeyedCollection?.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
		}

		/// <summary>
		/// Gets the index of the key specified.
		/// </summary>
		/// <param name="key">The key whose index will be located</param>
		/// <returns>Returns the index of the key specified if found.  Returns -1 if the key could not be located.</returns>
		public int IndexOf(TKey key)
		{
			if (_delegatedKeyedCollection!.Contains(key))
			{
				return _delegatedKeyedCollection.IndexOf(_delegatedKeyedCollection[key]);
			}

			return -1;
		}

		/// <summary>
		/// Determines whether this object contains the specified value.
		/// </summary>
		/// <param name="value">The value to locate in this object.</param>
		/// <returns>True if the value is found.  False otherwise.</returns>
		public bool ContainsValue(TValue value) => Values.Contains(value);

		/// <summary>
		/// Determines whether this object contains the specified value.
		/// </summary>
		/// <param name="value">The value to locate in this object.</param>
		/// <param name="comparer">The equality comparer used to locate the specified value in this object.</param>
		/// <returns>True if the value is found.  False otherwise.</returns>
		public bool ContainsValue(TValue value, IEqualityComparer<TValue> comparer) => Values.Contains(value, comparer);

		/// <summary>
		/// Determines whether this object contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in this object.</param>
		/// <returns>True if the key is found.  False otherwise.</returns>
		public bool ContainsKey(TKey key) => _delegatedKeyedCollection!.Contains(key);

		/// <summary>
		/// Returns the KeyValuePair at the index specified.
		/// </summary>
		/// <param name="index">The index of the KeyValuePair desired</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when the index specified does not refer to a KeyValuePair in this object
		/// </exception>
		public KeyValuePair<TKey, TValue> GetItem(int index)
		{
			if (index < 0 || index >= _delegatedKeyedCollection!.Count)
			{
				throw new ArgumentException($"The index was outside the bounds of the dictionary: {index}");
			}

			return _delegatedKeyedCollection[index];
		}

		/// <summary>
		/// Sets the value at the index specified.
		/// </summary>
		/// <param name="index">The index of the value desired</param>
		/// <param name="value">The value to set</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when the index specified does not refer to a KeyValuePair in this object
		/// </exception>
		public void SetItem(int index, TValue value)
		{
			if (index < 0 || index >= _delegatedKeyedCollection!.Count)
			{
				throw new ArgumentException($"The index is outside the bounds of the dictionary: {index}");
			}

			var kvp = new KeyValuePair<TKey, TValue>(_delegatedKeyedCollection[index].Key, value);
			_delegatedKeyedCollection[index] = kvp;
		}

		/// <summary>
		/// Returns an enumerator that iterates through all the KeyValuePairs in this object.
		/// </summary>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _delegatedKeyedCollection!.GetEnumerator();

		/// <summary>
		/// Removes the key-value pair for the specified key.
		/// </summary>
		/// <param name="key">The key to remove from the dictionary.</param>
		/// <returns>True if the item specified existed and the removal was successful.  False otherwise.</returns>
		public bool Remove(TKey key) => _delegatedKeyedCollection!.Remove(key);

		/// <summary>
		/// Removes the key-value pair at the specified index.
		/// </summary>
		/// <param name="index">The index of the key-value pair to remove from the dictionary.</param>
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= _delegatedKeyedCollection!.Count)
			{
				throw new ArgumentException($"The index was outside the bounds of the dictionary: {index}");
			}

			_delegatedKeyedCollection.RemoveAt(index);
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key associated with the value to get.</param>
		public TValue GetValue(TKey key)
		{
			if (_delegatedKeyedCollection!.Contains(key) == false)
			{
				throw new ArgumentException($"The given key is not present in the dictionary: {key}");
			}

			var kvp = _delegatedKeyedCollection[key];
			return kvp.Value;
		}

		/// <summary>
		/// Sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key associated with the value to set.</param>
		/// <param name="value">The the value to set.</param>
		public void SetValue(TKey key, TValue value)
		{
			var kvp = new KeyValuePair<TKey, TValue>(key, value);
			var idx = IndexOf(key);
			if (idx > -1)
			{
				_delegatedKeyedCollection![idx] = kvp;
			}
			else
			{
				_delegatedKeyedCollection!.Add(kvp);
			}
		}

		/// <summary>
		/// Tries to get the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the desired element.</param>
		/// <param name="value">
		/// When this method returns, contains the value associated with the specified key if
		/// that key was found.  Otherwise it will contain the default value for parameter's type.
		/// This parameter should be provided uninitialized.
		/// </param>
		/// <returns>True if the value was found.  False otherwise.</returns>
		/// <remarks></remarks>
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (_delegatedKeyedCollection!.Contains(key))
			{
				value = _delegatedKeyedCollection[key].Value;
				return true;
			}

			value = default!;
			return false;
		}

		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			Add(key, value);
		}

		bool IDictionary<TKey, TValue>.ContainsKey(TKey key) => ContainsKey(key);

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

		bool IDictionary<TKey, TValue>.Remove(TKey key) => Remove(key);

		bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => TryGetValue(key, out value);

		ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

		TValue IDictionary<TKey, TValue>.this[TKey key]
		{
			get => this[key];
			set => this[key] = value;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			_delegatedKeyedCollection!.Add(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			_delegatedKeyedCollection!.Clear();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
			=> _delegatedKeyedCollection!.Contains(item);

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			_delegatedKeyedCollection!.CopyTo(array, arrayIndex);
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count => _delegatedKeyedCollection!.Count;

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
			=> _delegatedKeyedCollection!.Remove(item);

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
			=> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IDictionaryEnumerator IOrderedDictionary.GetEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);

		void IOrderedDictionary.Insert(int index, object key, object value)
		{
			Insert(index, (TKey)key, (TValue)value);
		}

		void IOrderedDictionary.RemoveAt(int index)
		{
			RemoveAt(index);
		}

		object? IOrderedDictionary.this[int index]
		{
			get => this[index];
			set => this[index] = (TValue)value!;
		}

		void IDictionary.Add(object key, object? value)
		{
			Add((TKey)key, (TValue)value!);
		}

		void IDictionary.Clear()
		{
			Clear();
		}

		bool IDictionary.Contains(object key) => _delegatedKeyedCollection!.Contains((TKey)key);

		IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);

		bool IDictionary.IsFixedSize => false;

		bool IDictionary.IsReadOnly => false;

		ICollection IDictionary.Keys => (ICollection)Keys;

		void IDictionary.Remove(object key)
		{
			Remove((TKey)key);
		}

		ICollection IDictionary.Values => (ICollection)Values;

		object? IDictionary.this[object key]
		{
			get => this[(TKey)key];
			set => this[(TKey)key] = (TValue)value!;
		}

		void ICollection.CopyTo(System.Array array, int index)
		{
			((ICollection)_delegatedKeyedCollection!).CopyTo(array, index);
		}

		int ICollection.Count => ((ICollection)_delegatedKeyedCollection!).Count;

		bool ICollection.IsSynchronized => ((ICollection)_delegatedKeyedCollection!).IsSynchronized;

		object ICollection.SyncRoot => ((ICollection)_delegatedKeyedCollection!).SyncRoot;
	}
}