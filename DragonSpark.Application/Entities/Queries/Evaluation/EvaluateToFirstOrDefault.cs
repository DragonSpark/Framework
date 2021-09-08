using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToFirstOrDefault<T> : EvaluateToFirstOrDefault<None, T>
	{
		/*public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToFirstOrDefault(IInvocations invocations,
		                                Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToFirstOrDefault(IInvocations invocations,
		                                Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToFirstOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirstOrDefault<TIn, T> : Evaluate<TIn, T, T?>
	{
		/*public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(contexts, query.Get()) {}*/

		public EvaluateToFirstOrDefault(IInvocations invocations,
		                                Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		public EvaluateToFirstOrDefault(IInvoke<TIn, T> invoke) : base(invoke, FirstOrDefault<T>.Default) {}
	}
}