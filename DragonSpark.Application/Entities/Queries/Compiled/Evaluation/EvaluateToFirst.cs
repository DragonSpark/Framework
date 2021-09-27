using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToFirst<T> : EvaluateToFirst<None, T>
	{
		protected EvaluateToFirst(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		protected EvaluateToFirst(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		protected EvaluateToFirst(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirst<TIn, T> : Evaluate<TIn, T, T>
	{
		protected EvaluateToFirst(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		protected EvaluateToFirst(IInvoke<TIn, T> invoke) : base(invoke, ToFirst<T>.Default) {}
	}
}