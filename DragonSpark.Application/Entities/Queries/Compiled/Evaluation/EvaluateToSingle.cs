using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToSingle<T> : EvaluateToSingle<None, T>
	{
		public EvaluateToSingle(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(scopes, expression.Then()) {}

		public EvaluateToSingle(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(scopes, expression) {}

		public EvaluateToSingle(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToSingle<TIn, T> : Evaluate<TIn, T, T>
	{
		protected EvaluateToSingle(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(scopes, expression)) {}

		protected EvaluateToSingle(IInvoke<TIn, T> invoke) : base(invoke, ToSingle<T>.Default) {}
	}
}