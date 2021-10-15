using DragonSpark.Compose;
using DragonSpark.Runtime.Objects;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections;

/// <summary>
/// ATTRIBUTION: https://msdn.microsoft.com/en-us/library/ms404549%28v=vs.110%29.aspx?f=255&amp;MSPPError=-2147217396
/// </summary>
/// <typeparam name="T"></typeparam>
public class KeyedByTypeCollection<T> : DelegatedKeyedCollection<Type, T>
{
	public KeyedByTypeCollection() : this(Empty<T>.Enumerable) {}

	public KeyedByTypeCollection(IEnumerable<T> items) : base(InstanceType<T>.Default.Get)
	{
		foreach (var obj in items)
		{
			Add(obj);
		}
	}

	protected sealed override void InsertItem(int index, T item)
	{
		var key = GetKeyForItem(item);
		if (Contains(key))
		{
			Dictionary.Verify().Remove(key);
		}

		base.InsertItem(index, item);
	}
}