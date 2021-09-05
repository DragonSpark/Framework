using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class EditInvocationComposer<TIn, T>
	{
		readonly IInvoke<TIn, T> _subject;

		public EditInvocationComposer(IInvoke<TIn, T> subject) => _subject = subject;

		public IEdit<TIn, Array<T>> Array() => new Edit<TIn, T, Array<T>>(_subject, ToArray<T>.Default);

		public IEdit<TIn, Lease<T>> Lease() => new Edit<TIn, T, Lease<T>>(_subject, ToLease<T>.Default);

		public IEdit<TIn, List<T>> List() => new Edit<TIn, T, List<T>>(_subject, ToList<T>.Default);

		public IEdit<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> new Edit<TIn, T, Dictionary<TKey, T>>(_subject, new ToDictionary<T, TKey>(key));

		public IEdit<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
			Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> new Edit<TIn, T, Dictionary<TKey, TValue>>(_subject, new ToDictionary<T, TKey, TValue>(key, value));

		public IEdit<TIn, T> Single() => new Edit<TIn, T, T>(_subject, Single<T>.Default);

		public IEdit<TIn, T?> SingleOrDefault() => new Edit<TIn, T, T?>(_subject, SingleOrDefault<T>.Default);

		public IEdit<TIn, T> First() => new Edit<TIn, T, T>(_subject, First<T>.Default);

		public IEdit<TIn, T?> FirstOrDefault() => new Edit<TIn, T, T?>(_subject, FirstOrDefault<T>.Default);
	}
}