using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToSingleOrDefault<T> : EvaluateToSingleOrDefault<None, T>
	{
		public EvaluateToSingleOrDefault(IInvocations invocations,
		                                 Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToSingleOrDefault(IInvocations invocations,
		                                 Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToSingleOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToSingleOrDefault<TIn, T> : Evaluate<TIn, T, T?>
	{
		protected EvaluateToSingleOrDefault(IInvocations invocations,
		                                    Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		protected EvaluateToSingleOrDefault(IInvoke<TIn, T> invoke) : base(invoke, ToSingleOrDefault<T>.Default) {}
	}
}