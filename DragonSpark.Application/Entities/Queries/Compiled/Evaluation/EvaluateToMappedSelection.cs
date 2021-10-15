using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToMappedSelection<T, TKey, TValue> : EvaluateToMappedSelection<None, T, TKey, TValue>
	where TKey : notnull
{
	// ReSharper disable once TooManyDependencies
	protected EvaluateToMappedSelection(IScopes scopes,
	                                    Expression<Func<DbContext, IQueryable<T>>> expression, Func<T, TKey> key,
	                                    Func<T, TValue> value)
		: base(scopes, expression.Then(), key, value) {}

	// ReSharper disable once TooManyDependencies
	protected EvaluateToMappedSelection(IScopes scopes,
	                                    Expression<Func<DbContext, None, IQueryable<T>>> expression,
	                                    Func<T, TKey> key,
	                                    Func<T, TValue> value)
		: base(scopes, expression, key, value) {}

	protected EvaluateToMappedSelection(IReading<None, T> reading, Func<T, TKey> key, Func<T, TValue> value)
		: base(reading, key, value) {}
}

public class EvaluateToMappedSelection<TIn, T, TKey, TValue> : Evaluate<TIn, T, Dictionary<TKey, TValue>>
	where TKey : notnull
{
	// ReSharper disable once TooManyDependencies
	protected EvaluateToMappedSelection(IScopes scopes,
	                                    Expression<Func<DbContext, TIn, IQueryable<T>>> expression,
	                                    Func<T, TKey> key, Func<T, TValue> value)
		: this(new Reading<TIn, T>(scopes, expression), key, value) {}

	protected EvaluateToMappedSelection(IReading<TIn, T> reading, Func<T, TKey> key, Func<T, TValue> value)
		: base(reading, new ToDictionary<T, TKey, TValue>(key, value)) {}
}