using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToSingleOrDefault<TContext, T> : EvaluateToSingleOrDefault<None, TContext, T>
		where TContext : DbContext
	{
		/*public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToSingleOrDefault(IContexts<TContext> contexts,
		                                 Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToSingleOrDefault(IContexts<TContext> contexts,
		                                 Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToSingleOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToSingleOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		/*public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(contexts, query.Get()) {}*/

		public EvaluateToSingleOrDefault(IContexts<TContext> contexts,
		                                 Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToSingleOrDefault(IInvoke<TIn, T> invoke) : base(invoke, SingleOrDefault<T>.Default) {}
	}
}