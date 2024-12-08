using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class EvaluateToNumber<T> : Evaluate<T, int, uint>
{
	protected EvaluateToNumber(IScopes scopes, Expression<Func<DbContext, T, IQueryable<int>>> expression)
		: this(new Reading<T, int>(scopes, expression)) {}

	protected EvaluateToNumber(IReading<T, int> reading) : base(reading, ToNumber.Default) {}
}