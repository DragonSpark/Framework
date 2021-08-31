using DragonSpark.Model;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToArray<TContext, T> : EvaluateToArray<None, TContext, T> where TContext : DbContext
	{
		/*public EvaluateToArray(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToArray(IContexts<TContext> contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(contexts, expression.Then()) {}

		public EvaluateToArray(IContexts<TContext> contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, expression) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToArray<TIn, TContext, T> : Evaluate<TIn, T, Array<T>> where TContext : DbContext
	{
		/*public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToArray(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TContext, TIn, T>(contexts, expression)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}
}