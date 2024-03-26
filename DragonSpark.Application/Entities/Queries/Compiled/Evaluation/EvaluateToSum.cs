using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToSum<T> : Evaluate<T, decimal, decimal>
{
	protected EvaluateToSum(IContexts contexts, Expression<Func<DbContext, T, IQueryable<decimal>>> expression)
		: this(new Reading<T, decimal>(contexts, expression)) {}

	protected EvaluateToSum(IReading<T, decimal> reading) : base(reading, ToSum.Default) {}
}