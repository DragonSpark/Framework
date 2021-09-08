using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToList<T> : EvaluateToList<None, T>
	{
		/*public EvaluateToList(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToList(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToList(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToList(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToList<TIn, T> : Evaluate<TIn, T, List<T>>
	{
		/*public EvaluateToList(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToList(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		public EvaluateToList(IInvoke<TIn, T> invoke) : base(invoke, ToList<T>.Default) {}
	}
}