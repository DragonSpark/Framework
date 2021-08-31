using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToList<TContext, T> : EvaluateToList<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToList(IContexts<TContext> contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToList(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToList<TIn, TContext, T> : Evaluate<TIn, T, List<T>> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToList(IInvoke<TIn, T> invoke) : base(invoke, ToList<T>.Default) {}
	}
}