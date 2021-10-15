using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Model.Sequences.Collections;

/// <summary>
/// ATTRIBUTION: https://github.com/mattmc3/dotmore
/// </summary>
public class DelegatedKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem> where TKey: notnull
{
	const    string            DelegateNullExceptionMessage = "Delegate passed cannot be null";
	readonly Func<TItem, TKey> _key;

	public DelegatedKeyedCollection(Func<TItem, TKey> key) : this(key, EqualityComparer<TKey>.Default) {}

	public DelegatedKeyedCollection(Func<TItem, TKey> key, IEqualityComparer<TKey> comparer) : base(comparer)
		=> _key = key ?? throw new InvalidOperationException(DelegateNullExceptionMessage);

	protected override TKey GetKeyForItem(TItem item) => _key(item);
}