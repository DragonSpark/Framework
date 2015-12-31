using System;
using System.Collections.Generic;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Runtime
{
	/// <summary>
	/// A dictionary of lists.
	/// </summary>
	/// <typeparam name="TKey">The key to use for lists.</typeparam>
	/// <typeparam name="TValue">The type of the value held by lists.</typeparam>
	public sealed class ListDictionary<TKey, TValue> : IDictionary<TKey, IList<TValue>>
	{
		readonly Dictionary<TKey, IList<TValue>> innerValues = new Dictionary<TKey, IList<TValue>>();

		#region Public Methods

		/// <summary>
		/// If a list does not already exist, it will be created automatically.
		/// </summary>
		/// <param name="key">The key of the list that will hold the value.</param>
		public void Add([Required]TKey key)
		{
			CreateNewList(key);
		}

		/// <summary>
		/// Adds a value to a list with the given key. If a list does not already exist, 
		/// it will be created automatically.
		/// </summary>
		/// <param name="key">The key of the list that will hold the value.</param>
		/// <param name="value">The value to add to the list under the given key.</param>
		public void Add([Required]TKey key, [Required]TValue value)
		{
			if (innerValues.ContainsKey(key))
			{
				innerValues[key].Add(value);
			}
			else
			{
				List<TValue> values = CreateNewList(key);
				values.Add(value);
			}
		}

		List<TValue> CreateNewList(TKey key)
		{
			var values = new List<TValue>();
			innerValues.Add(key, values);
			return values;
		}

		/// <summary>
		/// Removes all entries in the dictionary.
		/// </summary>
		public void Clear()
		{
			innerValues.Clear();
		}

		/*/// <summary>
		/// Determines whether the dictionary contains the specified value.
		/// </summary>
		/// <param name="value">The value to locate.</param>
		/// <returns>true if the dictionary contains the value in any list; otherwise, false.</returns>
		public bool ContainsValue(TValue value)
		{
			var result = innerValues.Any( pair => pair.Value.Contains( value ) );
			return result;
		}*/

		/// <summary>
		/// Determines whether the dictionary contains the given key.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <returns>true if the dictionary contains the given key; otherwise, false.</returns>
		public bool ContainsKey([Required]TKey key)
		{
			return innerValues.ContainsKey(key);
		}

		/*/// <summary>
		/// Retrieves the all the elements from the list which have a key that matches the condition 
		/// defined by the specified predicate.
		/// </summary>
		/// <param name="keyFilter">The filter with the condition to use to filter lists by their key.</param>
		/// <returns>The elements that have a key that matches the condition defined by the specified predicate.</returns>
		public IEnumerable<TValue> FindAllValuesByKey(Predicate<TKey> keyFilter)
		{
			var result = this.Where( pair => keyFilter(pair.Key) ).SelectMany( pair => pair.Value );
			return result;
		}*/

		/*/// <summary>
		/// Retrieves all the elements that match the condition defined by the specified predicate.
		/// </summary>
		/// <param name="valueFilter">The filter with the condition to use to filter values.</param>
		/// <returns>The elements that match the condition defined by the specified predicate.</returns>
		public IEnumerable<TValue> FindAllValues(Predicate<TValue> valueFilter)
		{
			return this
				.SelectMany( pair => pair.Value, ( pair, value ) => new { pair, value } )
				.Where( @t => valueFilter( @t.value ) )
				.Select( @t => @t.value );
		}*/

		/// <summary>
		/// Removes a list by key.
		/// </summary>
		/// <param name="key">The key of the list to remove.</param>
		/// <returns><see langword="true" /> if the element was removed.</returns>
		public bool Remove([Required]TKey key)
		{
			return innerValues.Remove(key);
		}

		/*/// <summary>
		/// Removes a value from the list with the given key.
		/// </summary>
		/// <param name="key">The key of the list where the value exists.</param>
		/// <param name="value">The value to remove.</param>
		void Remove(TKey key, TValue value)
		{
			if (key == null)
				throw new ArgumentNullException(nameof( key ));

			if (value == null)
				throw new ArgumentNullException(nameof( value ));

			if (innerValues.ContainsKey(key))
			{
				List<TValue> innerList = (List<TValue>)innerValues[key];
				innerList.RemoveAll( item => value.Equals( item ) );
			}
		}*/

		/*/// <summary>
		/// Removes a value from all lists where it may be found.
		/// </summary>
		/// <param name="value">The value to remove.</param>
		public void Remove(TValue value)
		{
			foreach (KeyValuePair<TKey, IList<TValue>> pair in innerValues)
			{
				Remove(pair.Key, value);
			}
		}*/
		#endregion

		#region Properties

		/*/// <summary>
		/// Gets a shallow copy of all values in all lists.
		/// </summary>
		/// <value>List of values.</value>
		public IList<TValue> Values
		{
			get
			{
				List<TValue> values = new List<TValue>();
				foreach (IEnumerable<TValue> list in innerValues.Values)
				{
					values.AddRange(list);
				}

				return values;
			}
		}*/

		/// <summary>
		/// Gets the list of keys in the dictionary.
		/// </summary>
		/// <value>Collection of keys.</value>
		public ICollection<TKey> Keys => innerValues.Keys;

		/// <summary>
		/// Gets or sets the list associated with the given key. The 
		/// access always succeeds, eventually returning an empty list.
		/// </summary>
		/// <param name="key">The key of the list to access.</param>
		/// <returns>The list associated with the key.</returns>
		public IList<TValue> this[TKey key]
		{
			get
			{
				if (innerValues.ContainsKey(key) == false)
				{
					innerValues.Add(key, new List<TValue>());
				}
				return innerValues[key];
			}
			set { innerValues[key] = value; }
		}

		/// <summary>
		/// Gets the number of lists in the dictionary.
		/// </summary>
		/// <value>Value indicating the values count.</value>
		public int Count => innerValues.Count;
		#endregion

		#region IDictionary<TKey,List<TValue>> Members
		/// <summary>
		/// See <see cref="IDictionary{TKey,TValue}.Add"/> for more information.
		/// </summary>
		void IDictionary<TKey, IList<TValue>>.Add([Required]TKey key, [Required]IList<TValue> value)
		{
			innerValues.Add(key, value);
		}

		/// <summary>
		/// See <see cref="IDictionary{TKey,TValue}.TryGetValue"/> for more information.
		/// </summary>
		bool IDictionary<TKey, IList<TValue>>.TryGetValue(TKey key, out IList<TValue> value)
		{
			value = this[key];
			return true;
		}

		/// <summary>
		/// See <see cref="IDictionary{TKey,TValue}.Values"/> for more information.
		/// </summary>
		ICollection<IList<TValue>> IDictionary<TKey, IList<TValue>>.Values => innerValues.Values;
		#endregion

		#region ICollection<KeyValuePair<TKey,List<TValue>>> Members
		/// <summary>
		/// See <see cref="ICollection{TValue}.Add"/> for more information.
		/// </summary>
		void ICollection<KeyValuePair<TKey, IList<TValue>>>.Add(KeyValuePair<TKey, IList<TValue>> item)
		{
			((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).Add(item);
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.Contains"/> for more information.
		/// </summary>
		bool ICollection<KeyValuePair<TKey, IList<TValue>>>.Contains(KeyValuePair<TKey, IList<TValue>> item)
		{
			return ((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).Contains(item);
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.CopyTo"/> for more information.
		/// </summary>
		void ICollection<KeyValuePair<TKey, IList<TValue>>>.CopyTo(KeyValuePair<TKey, IList<TValue>>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// See <see cref="ICollection{TValue}.IsReadOnly"/> for more information.
		/// </summary>
		bool ICollection<KeyValuePair<TKey, IList<TValue>>>.IsReadOnly => ((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).IsReadOnly;

		/// <summary>
		/// See <see cref="ICollection{TValue}.Remove"/> for more information.
		/// </summary>
		bool ICollection<KeyValuePair<TKey, IList<TValue>>>.Remove(KeyValuePair<TKey, IList<TValue>> item)
		{
			return ((ICollection<KeyValuePair<TKey, IList<TValue>>>)innerValues).Remove(item);
		}
		#endregion

		#region IEnumerable<KeyValuePair<TKey,List<TValue>>> Members
		/// <summary>
		/// See <see cref="IEnumerable{TValue}.GetEnumerator"/> for more information.
		/// </summary>
		public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
		{
			return innerValues.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		/// See <see cref="System.Collections.IEnumerable.GetEnumerator"/> for more information.
		/// </summary>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerValues.GetEnumerator();
		}
		#endregion
	}
}