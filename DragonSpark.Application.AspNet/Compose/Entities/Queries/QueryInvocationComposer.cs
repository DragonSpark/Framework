using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Compose.Entities.Queries;

public sealed class QueryInvocationComposer<TIn, T>
{
	readonly IScopes          _scopes;
	readonly IQuery<TIn, T>   _query;
	readonly IReading<TIn, T> _subject;

	public QueryInvocationComposer(IScopes scopes, IQuery<TIn, T> query)
		: this(scopes, query, new Reading<TIn, T>(scopes, query.Get())) {}

	public QueryInvocationComposer(IScopes scopes, IQuery<TIn, T> query, IReading<TIn, T> subject)
	{
		_scopes  = scopes;
		_query   = query;
		_subject = subject;
	}

	public IStopAware<TIn, Array<T>> Array() => new Evaluate<TIn, T, Array<T>>(_subject, ToArray<T>.Default);

	public IStopAware<TIn, Leasing<T>> Lease() => new Evaluate<TIn, T, Leasing<T>>(_subject, ToLease<T>.Default);

	public IStopAware<TIn, List<T>> List() => new Evaluate<TIn, T, List<T>>(_subject, ToList<T>.Default);

	public IStopAware<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
		=> new Evaluate<TIn, T, Dictionary<TKey, T>>(_subject, new ToDictionary<T, TKey>(key));

	public IStopAware<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
		Func<T, TKey> key, Func<T, TValue> value)
		where TKey : notnull
		=> new Evaluate<TIn, T, Dictionary<TKey, TValue>>(_subject, new ToDictionary<T, TKey, TValue>(key, value));

	public IStopAware<TIn, T> Single() => new EvaluateToSingle<TIn, T>(_scopes, _query.Get());

	public IStopAware<TIn, T?> SingleOrDefault() => new EvaluateToSingleOrDefault<TIn, T>(_scopes, _query.Get());

	public IStopAware<TIn, T> First() => new EvaluateToFirst<TIn, T>(_scopes, _query.Get());

	public IStopAware<TIn, T?> FirstOrDefault() => new EvaluateToFirstOrDefault<TIn, T>(_scopes, _query.Get());

	public IStopAware<TIn, bool> Any() => new EvaluateToAny<TIn, T>(_scopes, _query.Get());
}