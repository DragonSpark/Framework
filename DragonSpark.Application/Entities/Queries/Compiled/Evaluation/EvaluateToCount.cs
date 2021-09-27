using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToCount<T> : EvaluateToCount<None, T>
	{
		public EvaluateToCount(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToCount(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToCount(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToCount<TIn, T> : Evaluate<TIn, T, uint>
	{
		public EvaluateToCount(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		public EvaluateToCount(IInvoke<TIn, T> invoke) : base(invoke, ToCount<T>.Default) {}
	}
}