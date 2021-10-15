using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class QueryInvocationComposer<TIn, T>
{
	readonly IReading<TIn, T> _subject;

	public QueryInvocationComposer(IReading<TIn, T> subject) => _subject = subject;

	public ISelecting<TIn, Array<T>> Array() => new Evaluate<TIn, T, Array<T>>(_subject, ToArray<T>.Default);

	public ISelecting<TIn, Leasing<T>> Lease() => new Evaluate<TIn, T, Leasing<T>>(_subject, ToLease<T>.Default);

	public ISelecting<TIn, List<T>> List() => new Evaluate<TIn, T, List<T>>(_subject, ToList<T>.Default);

	public ISelecting<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
		=> new Evaluate<TIn, T, Dictionary<TKey, T>>(_subject, new ToDictionary<T, TKey>(key));

	public ISelecting<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
		Func<T, TKey> key, Func<T, TValue> value)
		where TKey : notnull
		=> new Evaluate<TIn, T, Dictionary<TKey, TValue>>(_subject, new ToDictionary<T, TKey, TValue>(key, value));

	public ISelecting<TIn, T> Single() => new Evaluate<TIn, T, T>(_subject, ToSingle<T>.Default);

	public ISelecting<TIn, T?> SingleOrDefault() => new Evaluate<TIn, T, T?>(_subject, ToSingleOrDefault<T>.Default);

	public ISelecting<TIn, T> First() => new Evaluate<TIn, T, T>(_subject, ToFirst<T>.Default);

	public ISelecting<TIn, T?> FirstOrDefault() => new Evaluate<TIn, T, T?>(_subject, ToFirstOrDefault<T>.Default);

	public ISelecting<TIn, bool> Any() => new Evaluate<TIn, T, bool>(_subject, ToAny<T>.Default);
}