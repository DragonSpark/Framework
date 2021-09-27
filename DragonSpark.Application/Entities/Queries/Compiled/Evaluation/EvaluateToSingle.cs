using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToSingle<T> : EvaluateToSingle<None, T>
	{
		public EvaluateToSingle(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToSingle(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToSingle(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToSingle<TIn, T> : Evaluate<TIn, T, T>
	{
		protected EvaluateToSingle(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		protected EvaluateToSingle(IInvoke<TIn, T> invoke) : base(invoke, ToSingle<T>.Default) {}
	}
}