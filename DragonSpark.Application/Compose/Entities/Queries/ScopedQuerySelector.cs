﻿using DragonSpark.Application.Entities.Queries.Runtime.Selection;
using DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize;
using System;
using System.Linq;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public class ScopedQuerySelector<TIn, T>
	{
		readonly IQuery<TIn, T> _subject;

		public ScopedQuerySelector(IQuery<TIn, T> subject) => _subject = subject;

		public ScopedQuerySelector<TIn, TTo> Select<TTo>(Func<IQueryable<T>, IQueryable<TTo>> selection)
			=> new(new Select<TIn, T, TTo>(_subject, selection));

		public Any<TIn, T> Any() => new(_subject);

		public Single<TIn, T> Single() => new(_subject);

		public SingleOrDefault<TIn, T> SingleOrDefault() => new(_subject);

		public First<TIn, T> First() => new(_subject);

		public FirstOrDefault<TIn, T> FirstOrDefault() => new(_subject);

		public ToArray<TIn, T> ToArray() => new(_subject);

		public ToList<TIn, T> ToList() => new(_subject);

		public ToDictionary<TIn, TKey, T> ToDictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> new(_subject, key);

		public ToDictionary<TIn, T, TKey, TValue> ToDictionary<TKey, TValue>(Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> new(_subject, key, value);
	}
}