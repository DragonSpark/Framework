using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

[UsedImplicitly]
public class EvaluateToList<TIn, T> : Evaluate<TIn, T, List<T>>
{
	protected EvaluateToList(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, expression)) {}

	protected EvaluateToList(IReading<TIn, T> reading) : base(reading, ToList<T>.Default) {}
}