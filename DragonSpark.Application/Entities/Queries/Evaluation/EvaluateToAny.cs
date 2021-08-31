using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToAny<TContext, T> : EvaluateToAny<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToAny(IContexts<TContext> contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToAny(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToAny<TIn, TContext, T> : Evaluate<TIn, T, bool> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToAny(IInvoke<TIn, T> invoke) : base(invoke, Any<T>.Default) {}
	}
}