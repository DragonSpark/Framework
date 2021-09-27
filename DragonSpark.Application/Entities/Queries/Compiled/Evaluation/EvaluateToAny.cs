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
		public EvaluateToAny(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToAny(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToAny(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToAny<TIn, T> : Evaluate<TIn, T, bool>, IDepending<TIn>
	{
		protected EvaluateToAny(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		protected EvaluateToAny(IInvoke<TIn, T> invoke) : base(invoke, ToAny<T>.Default) {}
	}
}