using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

[UsedImplicitly]
public class EvaluateToList<TIn, T> : Evaluate<TIn, T, List<T>>
{
	protected EvaluateToList(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToList(IReading<TIn, T> reading) : base(reading, ToList<T>.Default) {}
}