using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToDecimal<T> : Evaluate<T, decimal, decimal>
{
	protected EvaluateToDecimal(IContexts contexts, Expression<Func<DbContext, T, IQueryable<decimal>>> expression)
		: this(new Reading<T, decimal>(contexts, expression)) {}

	protected EvaluateToDecimal(IReading<T, decimal> reading) : base(reading, ToDecimal.Default) {}
}