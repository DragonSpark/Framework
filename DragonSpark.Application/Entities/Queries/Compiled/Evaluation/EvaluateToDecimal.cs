using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToDecimal<T> : Evaluate<T, decimal, decimal>
	{
		public EvaluateToDecimal(IScopes scopes, Expression<Func<DbContext, T, IQueryable<decimal>>> expression)
			: this(new Reading<T, decimal>(scopes, expression)) {}

		public EvaluateToDecimal(IReading<T, decimal> reading) : base(reading, ToDecimal.Default) {}
	}
}