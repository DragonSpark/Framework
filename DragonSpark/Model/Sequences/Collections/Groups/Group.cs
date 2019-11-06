using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	public class Group<T> : IGroup<T>
	{
		readonly IList<T> _collection;

		public Group(GroupName name) : this(name, Enumerable.Empty<T>()) {}

		public Group(GroupName name, params T[] items) : this(name, items.AsEnumerable()) {}

		public Group(GroupName name, IEnumerable<T> items) : this(name, items.ToList()) {}

		public Group(GroupName name, IList<T> collection)
		{
			Name        = name;
			_collection = collection;
		}

		public GroupName Name { get; }

		public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item)
		{
			_collection.Add(item);
		}

		public void Clear()
		{
			_collection.Clear();
		}

		public bool Contains(T item) => _collection.Contains(item);

		public void CopyTo(T[] array, int arrayIndex)
		{
			_collection.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item) => _collection.Remove(item);

		public int Count => _collection.Count;

		public bool IsReadOnly => _collection.IsReadOnly;

		public int IndexOf(T item) => _collection.IndexOf(item);

		public void Insert(int index, T item)
		{
			_collection.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_collection.RemoveAt(index);
		}

		public T this[int index]
		{
			get => _collection[index];
			set => _collection[index] = value;
		}
	}
}