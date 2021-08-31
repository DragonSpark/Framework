using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToFirst<TContext, T> : EvaluateToFirst<None, TContext, T> where TContext : DbContext
	{
		/*public EvaluateToFirst(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToFirst(IContexts<TContext> contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToFirst(IContexts<TContext> contexts,
		                       Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToFirst(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirst<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		/*public EvaluateToFirst(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToFirst(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToFirst(IInvoke<TIn, T> invoke) : base(invoke, First<T>.Default) {}
	}
}