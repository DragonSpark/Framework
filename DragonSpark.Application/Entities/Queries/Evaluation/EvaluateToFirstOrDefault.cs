using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToFirstOrDefault<TContext, T> : EvaluateToFirstOrDefault<None, TContext, T>
		where TContext : DbContext
	{
		/*public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToFirstOrDefault(IContexts<TContext> contexts,
		                                Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToFirstOrDefault(IContexts<TContext> contexts,
		                                Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToFirstOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirstOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		/*public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(contexts, query.Get()) {}*/

		public EvaluateToFirstOrDefault(IContexts<TContext> contexts,
		                                Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToFirstOrDefault(IInvoke<TIn, T> invoke) : base(invoke, FirstOrDefault<T>.Default) {}
	}
}