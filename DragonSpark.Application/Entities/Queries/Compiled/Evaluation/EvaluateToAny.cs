using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToAny<T> : EvaluateToAny<None, T>
	{
		/*public EvaluateToAny(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToAny(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToAny(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToAny(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToAny<TIn, T> : Evaluate<TIn, T, bool>, IDepending<TIn>
	{
		/*public EvaluateToAny(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToAny(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		public EvaluateToAny(IInvoke<TIn, T> invoke) : base(invoke, Any<T>.Default) {}
	}
}