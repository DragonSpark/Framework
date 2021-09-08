using DragonSpark.Model;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToArray<T> : EvaluateToArray<None, T>
	{
		/*public EvaluateToArray(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToArray(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToArray(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToArray<TIn, T> : Evaluate<TIn, T, Array<T>>
	{
		/*public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToArray(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}
}