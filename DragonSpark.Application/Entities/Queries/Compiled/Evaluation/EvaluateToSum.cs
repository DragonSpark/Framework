using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToSum<T> : Evaluate<T, decimal, decimal>
{
	protected EvaluateToSum(IScopes scopes, Expression<Func<DbContext, T, IQueryable<decimal>>> expression)
		: this(new Reading<T, decimal>(scopes, expression)) {}

	protected EvaluateToSum(IReading<T, decimal> reading) : base(reading, ToSum.Default) {}
}