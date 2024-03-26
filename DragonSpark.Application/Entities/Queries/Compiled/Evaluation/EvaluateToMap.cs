using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToMap<TIn, T, TKey> : Evaluate<TIn, T, Dictionary<TKey, T>>
	where TKey : notnull
{
	protected EvaluateToMap(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression,
	                        Func<T, TKey> key)
		: this(new Reading<TIn, T>(contexts, expression), new ToDictionary<T, TKey>(key)) {}

	protected EvaluateToMap(IReading<TIn, T> reading, IEvaluate<T, Dictionary<TKey, T>> evaluate)
		: base(reading, evaluate) {}
}