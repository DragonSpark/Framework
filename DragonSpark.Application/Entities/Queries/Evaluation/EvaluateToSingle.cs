using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToSingle<TContext, T> : EvaluateToSingle<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts,
		                        Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToSingle(IContexts<TContext> contexts,
		                        Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToSingle(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToSingle<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts,
		                        Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToSingle(IInvoke<TIn, T> invoke) : base(invoke, Single<T>.Default) {}
	}
}