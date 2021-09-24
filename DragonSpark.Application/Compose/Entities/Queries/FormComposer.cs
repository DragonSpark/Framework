using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class FormComposer<TIn, T>
	{
		readonly IForm<TIn, T> _subject;

		public FormComposer(IForm<TIn, T> subject) => _subject = subject;
		public IForming<TIn, Array<T>> Array() => new Forming<TIn, T, Array<T>>(_subject, ToArray<T>.Default);

		public IForming<TIn, Leasing<T>> Lease() => new Forming<TIn, T, Leasing<T>>(_subject, ToLease<T>.Default);

		public IForming<TIn, List<T>> List() => new Forming<TIn, T, List<T>>(_subject, ToList<T>.Default);

		public IForming<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> new Forming<TIn, T, Dictionary<TKey, T>>(_subject, new ToDictionary<T, TKey>(key));

		public IForming<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
			Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> new Forming<TIn, T, Dictionary<TKey, TValue>>(_subject, new ToDictionary<T, TKey, TValue>(key, value));

		public IForming<TIn, T> Single() => new Forming<TIn, T, T>(_subject, Single<T>.Default);

		public IForming<TIn, T?> SingleOrDefault() => new Forming<TIn, T, T?>(_subject, SingleOrDefault<T>.Default);

		public IForming<TIn, T> First() => new Forming<TIn, T, T>(_subject, First<T>.Default);

		public IForming<TIn, T?> FirstOrDefault() => new Forming<TIn, T, T?>(_subject, FirstOrDefault<T>.Default);

		public IForming<TIn, bool> Any() => new Forming<TIn, T, bool>(_subject, Any<T>.Default);

	}
}