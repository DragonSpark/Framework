using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToAverage<T> : Evaluate<T, decimal, decimal>
{
	protected EvaluateToAverage(IScopes scopes, Expression<Func<DbContext, T, IQueryable<decimal>>> expression)
		: this(new Reading<T, decimal>(scopes, expression)) {}

	protected EvaluateToAverage(IReading<T, decimal> reading) : base(reading, ToAverage.Default) {}
}