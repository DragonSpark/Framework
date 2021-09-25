using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToFirst<T> : EvaluateToFirst<None, T>
	{
		/*public EvaluateToFirst(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToFirst(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToFirst(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToFirst(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirst<TIn, T> : Evaluate<TIn, T, T>
	{
		/*public EvaluateToFirst(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToFirst(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		public EvaluateToFirst(IInvoke<TIn, T> invoke) : base(invoke, ToFirst<T>.Default) {}
	}
}