using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToFirstOrDefault<T> : EvaluateToFirstOrDefault<None, T>
	{
		protected EvaluateToFirstOrDefault(IInvocations invocations,
		                                   Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		protected EvaluateToFirstOrDefault(IInvocations invocations,
		                                   Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		protected EvaluateToFirstOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirstOrDefault<TIn, T> : Evaluate<TIn, T, T?>
	{
		protected EvaluateToFirstOrDefault(IInvocations invocations,
		                                   Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		protected EvaluateToFirstOrDefault(IInvoke<TIn, T> invoke) : base(invoke, ToFirstOrDefault<T>.Default) {}
	}
}