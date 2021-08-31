using DragonSpark.Model;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToLease<TContext, T> : EvaluateToLease<None, TContext, T> where TContext : DbContext
	{
		/*public EvaluateToLease(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToLease(IContexts<TContext> contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToLease(IContexts<TContext> contexts,
		                       Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToLease(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToLease<TIn, TContext, T> : Evaluate<TIn, T, Lease<T>> where TContext : DbContext
	{
		/*public EvaluateToLease(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToLease(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToLease(IInvoke<TIn, T> invoke) : base(invoke, ToLease<T>.Default) {}
	}
}