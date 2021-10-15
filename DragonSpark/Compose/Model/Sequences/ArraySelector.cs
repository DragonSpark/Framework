using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose.Model.Sequences;

public class ArraySelector<_, T> : OpenArraySelector<_, T>
{
	readonly ISelect<_, Array<T>> _subject;

	public ArraySelector(ISelect<_, Array<T>> subject) : base(subject.Open()) => _subject = subject;

	public new Selector<_, Array<T>> Subject => new Selector<_, Array<T>>(_subject);

	public ISelect<_, IArrayMap<TKey, T>> GroupMap<TKey>(ISelect<T, TKey> key)
		where TKey : notnull
		=> GroupMap(key, EqualityComparer<TKey>.Default);

	public ISelect<_, IArrayMap<TKey, T>> GroupMap<TKey>(ISelect<T, TKey> key, IEqualityComparer<TKey> comparer)
		where TKey : notnull
		=> GroupMap(key.Get, comparer);

	public ISelect<_, IArrayMap<TKey, T>> GroupMap<TKey>(Func<T, TKey> key)
		where TKey : notnull
		=> GroupMap(key, EqualityComparer<TKey>.Default);

	public ISelect<_, IArrayMap<TKey, T>> GroupMap<TKey>(Func<T, TKey> key, IEqualityComparer<TKey> comparer)
		where TKey : notnull
		=> base.Subject.Select(x => new GroupMapAdapter<T, TKey>(new GroupMap<T, TKey>(key, comparer)).Get(x))
		       .Get();
}