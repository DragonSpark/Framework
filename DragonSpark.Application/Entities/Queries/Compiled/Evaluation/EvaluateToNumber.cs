using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToNumber<T> : Evaluate<T, int, uint>
{
	protected EvaluateToNumber(IContexts contexts, Expression<Func<DbContext, T, IQueryable<int>>> expression)
		: this(new Reading<T, int>(contexts, expression)) {}

	protected EvaluateToNumber(IReading<T, int> reading) : base(reading, ToNumber.Default) {}
}